namespace Caiman.Editor.Construction.Restrictions;

public class AreaRestrictionState
{
    public int ElementId;
    public double MinArea;

    public override string ToString() => $"Area Restriction:\n\tElement Id: {ElementId}\n\tMin Area: {MinArea}";
}
