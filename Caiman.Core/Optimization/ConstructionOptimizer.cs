using Caiman.Core.Analysis;
using Caiman.Core.Construction;
using Caiman.Core.Optimization.Restrictions;
using MathNet.Numerics.LinearAlgebra;

namespace Caiman.Core.Optimization;

public class ConstructionOptimizer(
    ConstructionAnalyzer analyzer,
    GradientFinder gradientFinder)
{
    private const double NormativeStepFactor = 0.2;

    public IList<double> Optimize(ConstructionEntity construction, IList<OptimizationRestriction> restrictions,
        OptimizationOptions options)
    {
        Vector<double> initialAreas = analyzer.GetAreasVector(construction);
        Vector<double> optimizedAreas = analyzer.GetAreasVector(construction);
        Func<IList<double>, double> targetFunc = analyzer.GenerateMaterialConsumptionFunction(construction);
        var initialValue = targetFunc(initialAreas);

        var iteration = 0;
        while (true)
        {
            foreach (var optimizationRestriction in restrictions)
            {
                optimizationRestriction.UpdateRestriction(optimizedAreas);
            }

            (IList<OptimizationRestriction> activeRestrictions, IList<OptimizationRestriction> passiveRestrictions,
                    IList<OptimizationRestriction> violatedRestrictions) =
                SortRestrictions(restrictions);

            var pointState = GetPointState(activeRestrictions, violatedRestrictions);

            if (pointState == PointState.Outside)
            {
                optimizedAreas = CorrectPoint(optimizedAreas, violatedRestrictions);
                continue;
            }

            Vector<double> targetAntiGradient = gradientFinder.FindAntiGradient(targetFunc, optimizedAreas);


            Vector<double> direction = pointState switch
            {
                PointState.Inside => targetAntiGradient,
                PointState.OnBoundary => AlongBoundaryDirection(targetAntiGradient, optimizedAreas, activeRestrictions),
                // Любой, потому что мы потом скорректируем точку, и снова найдем направление
                _ => targetAntiGradient,
            };

            var directionLength = direction.L2Norm();
            if (directionLength == 0)
            {
                break;
            }

            Vector<double> normDirection = direction / directionLength; // TODO проверка ортогональности векторов

            var step = GetStep(normDirection, optimizedAreas, passiveRestrictions);
            if (options.UsePrecision && options.CheckPrecision(step))
            {
                // if (options.UseCorrectRestrictions)
                // {
                //     if (violatedRestrictions.Count > 0)
                //     {
                //         continue;
                //     }
                // }

                break;
            }

            optimizedAreas += step * normDirection;
            if (options.UseMaxIterations && options.CheckIterations(iteration))
            {
                // if (options.UseCorrectRestrictions)
                // {
                //     if (violatedRestrictions.Count > 0)
                //     {
                //         continue;
                //     }
                // }

                break;
            }

            var currentValue = targetFunc(optimizedAreas);
            var currentEfficiency = Math.Abs(currentValue - initialValue) /
                initialValue;
            if (options.UseEfficiencyPerStep && options.CheckEfficiencyPerStep(currentEfficiency))
            {
                // if (options.UseCorrectRestrictions)
                // {
                //     if (violatedRestrictions.Count > 0)
                //     {
                //         continue;
                //     }
                // }

                break;
            }

            iteration++;
        }

        var efficiency = Math.Abs(targetFunc(optimizedAreas) - initialValue) / initialValue * 100;
        Console.WriteLine($"Эффективность {efficiency}");

        return optimizedAreas;
    }

    private (
        IList<OptimizationRestriction> ActiveRestrictions,
        IList<OptimizationRestriction> PassiveRestrictions,
        IList<OptimizationRestriction> ViolatedRestrictions) SortRestrictions(
            IList<OptimizationRestriction> restrictions)
    {
        IList<OptimizationRestriction> active = restrictions.Where(r => r.Type is RestrictionType.Active).ToList();
        IList<OptimizationRestriction> passive = restrictions.Where(r => r.Type is RestrictionType.Passive).ToList();
        IList<OptimizationRestriction> violated = restrictions.Where(r => r.Type is RestrictionType.Violated).ToList();
        return (active, passive, violated);
    }

    private double GetStep(Vector<double> directionNorm, Vector<double> areas,
        IList<OptimizationRestriction> passiveRestrictions)
    {
        var normativeStep = NormativeStepFactor * areas.L2Norm();
        Vector<double> passiveStep = Vector<double>.Build.Dense(passiveRestrictions.Count);
        for (var i = 0; i < passiveRestrictions.Count; i++)
        {
            var passiveRestriction = passiveRestrictions[i];
            passiveStep[i] = -passiveRestriction.Value / gradientFinder
                .FindGradient(passiveRestriction.GetRestrictionFunc(), areas).DotProduct(directionNorm);
        }


        var stepsCount = passiveStep.Count(d => d > 0);
        if (stepsCount == 0)
        {
            return normativeStep;
        }

        var minPassiveStep = passiveStep.Where(d => d > 0).Min();
        var step = Math.Min(normativeStep, minPassiveStep);
        return step;
    }

    private PointState GetPointState(IEnumerable<OptimizationRestriction> activeRestrictions,
        IEnumerable<OptimizationRestriction> violatedRestrictions)
    {
        var pointState = activeRestrictions.Count() switch
        {
            0 => PointState.Inside,
            > 0 => PointState.OnBoundary,
            _ => throw new Exception("Point State does not exist"),
        };

        if (violatedRestrictions.Any())
        {
            pointState = PointState.Outside;
        }

        return pointState;
    }


    private enum PointState
    {
        Inside,
        Outside,
        OnBoundary,
    }

    #region Direction Correct

    private Vector<double> AlongBoundaryDirection(
        Vector<double> targetAntiGradient,
        IList<double> areas,
        IList<OptimizationRestriction> activeRestrictions)
    {
        Vector<double> direction;
        if (activeRestrictions.Count == 1)
        {
            Vector<double> restrictionAntiGradient =
                gradientFinder.FindAntiGradient(activeRestrictions[0].GetRestrictionFunc(), areas);
            var lambda = -restrictionAntiGradient.DotProduct(targetAntiGradient) /
                restrictionAntiGradient.DotProduct(restrictionAntiGradient);
            direction = targetAntiGradient + restrictionAntiGradient * lambda;
            return direction;
        }

        Matrix<double> restrictionsAntiGradients = GetActiveRestrictionsMatrix(areas, activeRestrictions);
        Vector<double> multipliers =
            FindLagrangeMultipliersForActiveRestrictions(targetAntiGradient, restrictionsAntiGradients);
        while (multipliers.Any(m => m < 0))
        {
            restrictionsAntiGradients.ClearColumn(multipliers.MinimumIndex());
            multipliers = FindLagrangeMultipliersForActiveRestrictions(targetAntiGradient, restrictionsAntiGradients);
        }

        direction = restrictionsAntiGradients * multipliers + targetAntiGradient;
        return direction;
    }

    private Vector<double> CorrectPoint(
        Vector<double> areas,
        IList<OptimizationRestriction> violatedRestrictions)
    {
        Matrix<double> violatedRestrictionsMatrix = GetViolatedRestrictionsMatrix(areas, violatedRestrictions);
        Vector<double> violatedRestrictionsVector =
            Vector<double>.Build.DenseOfEnumerable(violatedRestrictions.Select(v => v.Value));
        Vector<double> multipliers =
            FindLagrangeMultipliersForViolatedRestrictions(violatedRestrictionsVector, violatedRestrictionsMatrix);
        Vector<double> delta = -violatedRestrictionsMatrix * multipliers;
        return areas + delta;
    }

    #endregion Direction Correct

    #region Helpers

    private Matrix<double> GetActiveRestrictionsMatrix(
        IList<double> areas,
        IList<OptimizationRestriction> activeRestrictions)
    {
        Matrix<double> restrictionsAntiGradients =
            Matrix<double>.Build.Dense(areas.Count, activeRestrictions.Count);
        // foreach (var (row, col, _) in restrictionsAntiGradients.EnumerateIndexed())
        // {
        //     restrictionsAntiGradients[row, col] =
        //         gradientFinder.FindAntiGradient(activeRestrictions[col].GetRestrictionFunc(), areas)[row];
        // }
        for (var i = 0; i < activeRestrictions.Count; i++)
        {
            Vector<double> gradient =
                gradientFinder.FindAntiGradient(activeRestrictions[i].GetRestrictionFunc(), areas);
            foreach (var (row, _) in restrictionsAntiGradients.EnumerateRowsIndexed())
            {
                restrictionsAntiGradients[row, i] = gradient[row];
            }
        }

        return restrictionsAntiGradients;
    }

    private Matrix<double> GetViolatedRestrictionsMatrix(
        IList<double> areas,
        IList<OptimizationRestriction> violatedRestrictions)
    {
        Matrix<double> restrictionsGradient =
            Matrix<double>.Build.Dense(areas.Count, violatedRestrictions.Count);
        for (var i = 0; i < violatedRestrictions.Count; i++)
        {
            Vector<double> gradient = gradientFinder.FindGradient(violatedRestrictions[i].GetRestrictionFunc(), areas);
            foreach (var (row, _) in restrictionsGradient.EnumerateRowsIndexed())
            {
                restrictionsGradient[row, i] = gradient[row];
            }
        }


        return restrictionsGradient;
    }


    #region Lagrange Multipliers

    private Vector<double> FindLagrangeMultipliersForActiveRestrictions(Vector<double> targetAntiGradient,
        Matrix<double> activeRestrictionsAntiGradients)
    {
        Matrix<double> restrictionsAntiGradientsTranspose = activeRestrictionsAntiGradients.Transpose();
        Vector<double> multipliers =
            -(restrictionsAntiGradientsTranspose * activeRestrictionsAntiGradients).Inverse() *
            restrictionsAntiGradientsTranspose * targetAntiGradient;
        return multipliers;
    }

    private Vector<double> FindLagrangeMultipliersForViolatedRestrictions(
        Vector<double> violatedRestrictionsVector,
        Matrix<double> violatedRestrictionsMatrix
    )
    {
        Vector<double> multipliers = -violatedRestrictionsVector *
            (-violatedRestrictionsMatrix.Transpose() * violatedRestrictionsMatrix).Inverse();

        return multipliers;
    }

    #endregion Lagrange Multipliers

    #endregion Helpers
}
