using Caiman.Core.Construction;

namespace Caiman.Editor.Construction.Restrictions;

public class NodeDisplacementRestrictionState
{
    public Axis Axis;
    public double MaxDisplacement;
    public int NodeId;

    public override string ToString() => $"Node Displacement Restriction:\n\tNode Id: {NodeId}\n\tMax Displacement: {MaxDisplacement}\n\tAxis: {Axis}";
}
