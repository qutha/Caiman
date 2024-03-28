using Caiman.Core.Construction;

namespace Caiman.Storage.Models;

public class ElementModel
{
    public NodeIndex StartNodeIndex { get; set; }

    public NodeIndex EndNodeIndex { get; set; }
    public double Elasticity { get; set; }
    public double Area { get; set; }

    public static ElementModel FromModel(ConstructionEntity construction, ElementEntity elementEntity) =>
        new()
        {
            StartNodeIndex = construction.Nodes.IndexOf(elementEntity.StartNodeEntity),
            EndNodeIndex = construction.Nodes.IndexOf(elementEntity.EndNodeEntity),
            Elasticity = elementEntity.Elasticity,
            Area = elementEntity.Area,
        };
}
