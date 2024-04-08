using Caiman.Core.Construction;

namespace Caiman.Storage.Models;

public class ElementModel
{
    public NodeIndex StartNodeIndex { get; set; }

    public NodeIndex EndNodeIndex { get; set; }
    public double Elasticity { get; set; }
    public double Area { get; set; }

    public static ElementModel FromModel(Construction construction, Element element) =>
        new()
        {
            StartNodeIndex = construction.Nodes.IndexOf(element.StartNode),
            EndNodeIndex = construction.Nodes.IndexOf(element.EndNode),
            Elasticity = element.Elasticity,
            Area = element.Area,
        };
}
