using Caiman.Core.Construction.Exceptions;

namespace Caiman.Core.Construction;

public class ConstructionEntity
{
    public List<NodeEntity> Nodes { get; set; } = [];
    public List<ElementEntity> Elements { get; set; } = [];

    public (NodeIndex StartIndex, NodeIndex EndIndex) GetNodesIndicesFromElement(ElementEntity elementEntity)
    {
        NodeIndex startIndex = Nodes.IndexOf(elementEntity.StartNodeEntity);
        NodeIndex endIndex = Nodes.IndexOf(elementEntity.EndNodeEntity);
        return (startIndex, endIndex);
    }

    public NodeIndex IndexOf(NodeEntity nodeEntity) => Nodes.IndexOf(nodeEntity);
    public ElementIndex IndexOf(ElementEntity elementEntity) => Elements.IndexOf(elementEntity);

    /// <summary>
    ///     Получение элемента по индексу
    /// </summary>
    /// <param name="index">Индекс элемента</param>
    /// <returns>Найденный элемент</returns>
    /// <exception cref="ElementNotFoundException">Элемент не найден</exception>
    public ElementEntity GetElement(ElementIndex index)
    {
        if (index >= Elements.Count || index < 0)
        {
            throw new ElementNotFoundException();
        }

        return Elements[index];
    }

    /// <summary>
    ///     Получение узла по индексу
    /// </summary>
    /// <param name="index">Индекс узла</param>
    /// <returns>Найденный узел</returns>
    /// <exception cref="NodeNotFoundException">Узел не найден</exception>
    public NodeEntity GetNode(NodeIndex index)
    {
        if (index >= Nodes.Count || index < 0)
        {
            throw new NodeNotFoundException();
        }

        return Nodes[index];
    }
}
