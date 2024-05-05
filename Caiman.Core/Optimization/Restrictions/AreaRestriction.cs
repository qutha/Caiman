namespace Caiman.Core.Optimization.Restrictions;

public class AreaRestriction(ElementIndex elementIndex, double minArea) : OptimizationRestriction
{
    protected override double GetValue(IList<double> areas) => minArea - areas[elementIndex];

    public static IList<AreaRestriction>
        CreateRestrictionForAll(Construction.Construction construction, double minArea) =>
        construction.Elements.Select((_, i) => new AreaRestriction(i, minArea)).ToArray();

    public override string ToString() => $"Area Restriction:\n\tElement {elementIndex}\n\tMin Area: {minArea}";
}
