namespace Caiman.Core.Optimization.Restrictions;

public class RestrictionBuilder
{
    private readonly List<OptimizationRestriction> _restrictions = [];

    public RestrictionBuilder Add(OptimizationRestriction restriction)
    {
        _restrictions.Add(restriction);
        return this;
    }

    public RestrictionBuilder Add(IEnumerable<OptimizationRestriction> restrictions)
    {
        _restrictions.AddRange(restrictions);
        return this;
    }

    public List<OptimizationRestriction> Build() => _restrictions;
}
