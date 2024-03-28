using Caiman.Core.Construction;

namespace Caiman.Storage.Models;

public class ConstructionModel
{
    public List<NodeModel> Nodes { get; set; } = [];
    public List<ElementModel> Elements { get; set; } = [];

    public static ConstructionModel FromEntity(ConstructionEntity construction)
    {
        var dto = new ConstructionModel
        {
            Nodes = construction.Nodes
                .Select(n => NodeModel.FromModel(construction, n))
                .ToList(),
            Elements = construction.Elements
                .Select(el => ElementModel.FromModel(construction, el))
                .ToList(),
        };
        return dto;
    }

    public ConstructionEntity ToEntity(ConstructionBuilder builder)
    {
        List<NodeEntity> nodes = Nodes.Select(n => new NodeEntity(n.X, n.Y)).ToList();
        foreach (NodeEntity node in nodes)
        {
            builder.AddNode(node);
        }

        IEnumerable<ElementEntity> elements = Elements.Select(el =>
            new ElementEntity(nodes[el.StartNodeIndex], nodes[el.EndNodeIndex], el.Elasticity, el.Area));
        foreach (ElementEntity element in elements)
        {
            builder.AddElement(element);
        }

        foreach (NodeModel nodeDto in Nodes.Where(n => n.Constraint is not null))
        {
            builder.AddConstraint(nodes[nodeDto.NodeIndex], nodeDto.Constraint!);
        }

        foreach (NodeModel nodeDto in Nodes.Where(n => n.LoadList.Count > 0))
        {
            foreach (ConcentratedLoad load in nodeDto.LoadList)
            {
                builder.AddLoad(nodes[nodeDto.NodeIndex], load);
            }
        }

        return builder.Build();
    }
}
