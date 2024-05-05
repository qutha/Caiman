using Caiman.Core.Construction;
using Caiman.Core.Construction.Exceptions;
using Caiman.Core.Matrices;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Caiman.Core.Analysis;

public class ConstructionAnalyzer(IMatrixBuilder matrixBuilder)
{
    #region Fields

    private const int Dimensions = 2;

    #endregion Fields

    #region Function Generators

    /// <summary>
    ///     Метод для генерации функции расхода материала от площадей стержней
    /// </summary>
    /// <param name="construction">Стержневая система</param>
    /// <returns>Функцию расхода материала от площадей стержней</returns>
    public Func<IList<double>, double> GenerateMaterialConsumptionFunction(Construction.Construction construction)
    {
        return Function;

        double Function(IList<double> areas) => areas.Select((t, i) => construction.Elements[i].Length * t).Sum();
    }

    /// <summary>
    ///     Метод для генерации функции перемещения узла вдоль оси от площадей стержней
    /// </summary>
    /// <param name="construction">Стержневая система</param>
    /// <param name="node">Индекс узла</param>
    /// <param name="axis">Ось, вдоль которой перемещается узел</param>
    /// <returns>Функция перемещения узла вдоль оси от площадей стержней</returns>
    /// <exception cref="NodeNotFoundException">Узел не находится внутри системы</exception>
    public Func<IList<double>, double> GenerateNodeDisplacementOnAreasFunction(Construction.Construction construction,
        Node node,
        Axis axis)
    {
        var nodeIndex = construction.IndexOf(node);

        return Function;

        double Function(IList<double> areas)
        {
            var initialAreas = construction.Elements.Select(el => el.Area).ToArray();
            for (var i = 0; i < areas.Count; i++)
            {
                construction.Elements[i].Area = areas[i];
            }

            Vector<double> vector = FindDisplacementVector(construction);
            var displacement = axis switch
            {
                Axis.X => vector[nodeIndex * Dimensions],
                Axis.Y => vector[nodeIndex * Dimensions + 1],

                _ => throw new InvalidOperationException("Это невозможно"),
            };

            for (var i = 0; i < areas.Count; i++)
            {
                construction.Elements[i].Area = initialAreas[i];
            }

            return displacement;
        }
    }

    #endregion Function Generators

    #region Analysis

    /// <summary>
    ///     Метод для нахождения вектора перемещения
    /// </summary>
    /// <param name="construction">Стержневая система</param>
    /// <returns>Вектор перемещения в см</returns>
    public Vector<double> FindDisplacementVector(Construction.Construction construction)
    {
        Matrix<double> constructionStiffnessMatrix =
            CreateGlobalConstructionStiffnessMatrixWithConstraints(construction);
        Vector<double> loadVector = GetLoadVector(construction);
        Matrix<double> inverseConstructionStiffnessMatrix = constructionStiffnessMatrix.Inverse();
        Vector<double> displacementVector = inverseConstructionStiffnessMatrix * loadVector;
        return displacementVector;
    }

    /// <summary>
    ///     <para>
    ///         <b>Метод для нахождения вектора внутренних усилий для стержня</b>
    ///     </para>
    ///     <para>Если вектор перемещения не задан, то произойдет расчет всей системы</para>
    ///     <para>Рекомендуется заранее вычислить вектор перемещения</para>
    /// </summary>
    /// <param name="construction">Стержневая система</param>
    /// <param name="element">Стержень, в котором требуется найти внутренние усилия</param>
    /// <param name="displacementVector">Вектор перемещения, если не задан, то произойдет расчет всей системы</param>
    /// <returns>Вектор внутренних усилий в кг</returns>
    /// <exception cref="ElementNotFoundException">Стержень не находится внутри системы</exception>
    public double FindInternalForces(Construction.Construction construction, Element element,
        Vector<double>? displacementVector = null)
    {
        Vector<double> displacement = displacementVector ?? FindDisplacementVector(construction);
        var (startIndex, endIndex) = construction.GetNodesIndicesFromElement(element);
        Vector<double>? elementDisplacement = Vector<double>.Build.DenseOfArray(
            [
                displacement[startIndex * Dimensions],
                displacement[startIndex * Dimensions + 1],
                displacement[endIndex * Dimensions],
                displacement[endIndex * Dimensions + 1],
            ]
        );
        Matrix<double> elementLocalStiffnessMatrix = CreateLocalElementStiffnessMatrix(element);
        Matrix<double> transformationMatrix =
            matrixBuilder.CreateTransformationMatrix(element.Sin, element.Cos);
        Vector<double> internalForces = elementLocalStiffnessMatrix * transformationMatrix * elementDisplacement;
        return internalForces[2];
    }

    /// <summary>
    ///     <para>
    ///         <b>Метод для нахождения напряжений в стержне</b>
    ///     </para>
    ///     <para>Если вектор внутренних усилий не задан, то произойдет расчет всей системы</para>
    ///     <para>Рекомендуется заранее вычислить вектор перемещения</para>
    /// </summary>
    /// <param name="construction">Стержневая система</param>
    /// <param name="element">Стержень, в котором требуется найти напряжения</param>
    /// <param name="internalForces">Вектор внутренних усилий, если не задан, то произойдет расчет всей системы</param>
    /// <returns>Вектор напряжений в кг/см2</returns>
    public double FindStresses(Construction.Construction construction, Element element,
        double? internalForces = null)
    {
        var internalForcesVector = internalForces ?? FindInternalForces(construction, element);
        var stresses = internalForcesVector / element.Area;
        return stresses;
    }

    public Vector<double> GetAreasVector(Construction.Construction construction)
    {
        Vector<double> areasVector = Vector<double>.Build.DenseOfEnumerable(
            construction.Elements.Select(el => el.Area));
        return areasVector;
    }

    public Vector<double> GetLoadVector(Construction.Construction construction)
    {
        Vector<double> loadVector = Vector<double>.Build.Sparse(construction.Nodes.Count * Dimensions);
        foreach (var node in construction.Nodes)
        {
            var nodeIndex = construction.IndexOf(node);
            var pos = nodeIndex * 2;
            loadVector[pos] = node.Loads.Sum(l => l.X);
            loadVector[pos + 1] = node.Loads.Sum(l => l.Y);
        }

        return loadVector;
    }

    #endregion Analysis

    #region Matrices

    public Matrix<double> CreateGlobalConstructionStiffnessMatrix(Construction.Construction construction)
    {
        var nodesCount = construction.Nodes.Count;
        var constructionStiffnessMatrix = SparseMatrix.Create(nodesCount * Dimensions, nodesCount * Dimensions, 0);
        foreach (var element in construction.Elements)
        {
            Matrix<double> elementStiffnessMatrix = CreateGlobalElementStiffnessMatrix(element);
            var (startNodeIndex, endNodeIndex) = construction.GetNodesIndicesFromElement(element);
            AddToConstructionStiffnessMatrix(constructionStiffnessMatrix, elementStiffnessMatrix, startNodeIndex,
                endNodeIndex);
        }

        return constructionStiffnessMatrix;
    }

    public Matrix<double> CreateGlobalConstructionStiffnessMatrixWithConstraints(Construction.Construction construction)
    {
        Matrix<double> globalStiffnessMatrix = CreateGlobalConstructionStiffnessMatrix(construction);
        ApplyConstraints(construction, globalStiffnessMatrix);
        return globalStiffnessMatrix;
    }

    public Matrix<double> CreateGlobalElementStiffnessMatrix(Element element)
    {
        Matrix<double> localStiffnessMatrix = CreateLocalElementStiffnessMatrix(element);
        Matrix<double> transformationMatrix =
            matrixBuilder.CreateTransformationMatrix(element.Sin, element.Cos);
        Matrix<double> globalStiffnessMatrix =
            transformationMatrix.Transpose() * localStiffnessMatrix * transformationMatrix;
        return globalStiffnessMatrix;
    }

    public Matrix<double> CreateLocalElementStiffnessMatrix(Element element)
    {
        Matrix<double> localStiffnessMatrix = element.Stiffness * matrixBuilder.CreateLocalMatrix();
        return localStiffnessMatrix;
    }

    #endregion Matrices

    #region Private Methods

    private void AddToConstructionStiffnessMatrix(Matrix<double> constructionStiffnessMatrix,
        Matrix<double> elementStiffnessMatrix, NodeIndex startNodeIndex, NodeIndex endNodeIndex)
    {
        foreach (var (row, col, value) in elementStiffnessMatrix.EnumerateIndexed())
        {
            var nodeRow = row < Dimensions
                ? startNodeIndex * Dimensions + row
                : endNodeIndex * Dimensions + row - Dimensions;
            var nodeCol = col < Dimensions
                ? startNodeIndex * Dimensions + col
                : endNodeIndex * Dimensions + col - Dimensions;
            constructionStiffnessMatrix[nodeRow, nodeCol] += value;
        }
    }

    private void ApplyConstraints(Construction.Construction construction, Matrix<double> constructionStiffnessMatrix)
    {
        foreach (var node in construction.Nodes)
        {
            var constraint = node.Constraint;
            var nodeIndex = construction.IndexOf(node);
            if (constraint.X)
            {
                var row = nodeIndex * Dimensions;
                var col = nodeIndex * Dimensions;
                constructionStiffnessMatrix.ClearColumn(col);
                constructionStiffnessMatrix.ClearRow(row);
                constructionStiffnessMatrix[row, col] = 1;
            }

            if (constraint.Y)
            {
                var row = nodeIndex * Dimensions + 1;
                var col = nodeIndex * Dimensions + 1;
                constructionStiffnessMatrix.ClearColumn(col);
                constructionStiffnessMatrix.ClearRow(row);
                constructionStiffnessMatrix[row, col] = 1;
            }
        }
    }

    #endregion Private Methods
}
