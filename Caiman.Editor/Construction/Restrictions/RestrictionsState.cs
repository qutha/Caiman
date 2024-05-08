namespace Caiman.Editor.Construction.Restrictions;

public class RestrictionsState
{
    public List<NodeDisplacementRestrictionState> NodeDisplacementRestrictionStates { get; set; } = new();
    public List<AreaRestrictionState> AreaRestrictionStates { get; set; } = new();
}
