using Caiman.Core.Construction;

namespace Caiman.Storage.Models;

public class NodeModel
{
    public NodeIndex NodeIndex { get; set; }
    public List<ConcentratedLoad> LoadList { get; set; } = null!;
    public Constraint? Constraint { get; set; }
    public double X { get; set; }

    public double Y { get; set; }

    public static NodeModel FromModel(ConstructionEntity construction, NodeEntity nodeEntity)
    {
        var dto = new NodeModel
        {
            NodeIndex = construction.Nodes.IndexOf(nodeEntity),
            LoadList = nodeEntity.LoadList,
            Constraint = nodeEntity.Constraint is null
                ? null
                : new Constraint(nodeEntity.Constraint.X, nodeEntity.Constraint.Y),
            X = nodeEntity.X,
            Y = nodeEntity.Y,
        };
        return dto;
    }
}
