using Caiman.Core.Optimization.Restrictions;

namespace Caiman.Editor.Construction.Restrictions;

public class RestrictionsState
{
    public List<OptimizationRestriction> Restrictions { get; set; } = new();
}
