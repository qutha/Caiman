using Caiman.Core.Construction.Exceptions;

namespace Caiman.Core.Construction;

public class Construction
{
    public List<Node> Nodes { get; set; } = [];
    public List<Element> Elements { get; set; } = [];

    public (NodeIndex StartIndex, NodeIndex EndIndex) GetNodesIndicesFromElement(Element element)
    {
        NodeIndex startIndex = Nodes.IndexOf(element.StartNode);
        NodeIndex endIndex = Nodes.IndexOf(element.EndNode);
        return (startIndex, endIndex);
    }

    public NodeIndex IndexOf(Node node) => Nodes.IndexOf(node);
    public ElementIndex IndexOf(Element element) => Elements.IndexOf(element);

    /// <summary>
    ///     Получение элемента по индексу
    /// </summary>
    /// <param name="index">Индекс элемента</param>
    /// <returns>Найденный элемент</returns>
    /// <exception cref="ElementNotFoundException">Элемент не найден</exception>
    public Element GetElement(ElementIndex index)
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
    public Node GetNode(NodeIndex index)
    {
        if (index >= Nodes.Count || index < 0)
        {
            throw new NodeNotFoundException();
        }

        return Nodes[index];
    }
}
