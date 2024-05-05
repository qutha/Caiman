using Caiman.Core.Construction;

namespace Caiman.Storage.Models;

public class NodeModel
{
    public NodeIndex NodeIndex { get; set; }
    public List<ConcentratedLoad> LoadList { get; set; } = null!;
    public Constraint? Constraint { get; set; }
    public double X { get; set; }

    public double Y { get; set; }

    public static NodeModel FromModel(Construction construction, Node node)
    {
        var dto = new NodeModel
        {
            NodeIndex = construction.Nodes.IndexOf(node),
            LoadList = node.Loads,
            Constraint = node.Constraint is null
                ? null
                : new Constraint(node.Constraint.X, node.Constraint.Y),
            X = node.X,
            Y = node.Y,
        };
        return dto;
    }
}
