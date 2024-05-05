namespace Caiman.Core.Optimization.Restrictions;

public class NodeDisplacementRestriction(
    Func<IList<double>, double> nodeDisplacementOnAreasFunction,
    double maxDisplacement)
    : OptimizationRestriction
{
    protected override double GetValue(IList<double> areas)
    {
        var displacementAbs = Math.Abs(nodeDisplacementOnAreasFunction(areas));
        return displacementAbs - maxDisplacement;
    }

    public override string ToString() => $"Node Displacement Restriction:\n\tMax Displacement: {maxDisplacement}";
}
