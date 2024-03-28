using Caiman.Core.Construction;

namespace Caiman.Core.Optimization.Restrictions;

public class AreaRestriction(ElementIndex elementIndex, double minArea) : OptimizationRestriction
{
    protected override double GetValue(IList<double> areas) => minArea - areas[elementIndex];

    public static IList<AreaRestriction> CreateRestrictionForAll(ConstructionEntity construction, double minArea) =>
        construction.Elements.Select((_, i) => new AreaRestriction(i, minArea)).ToArray();
}
