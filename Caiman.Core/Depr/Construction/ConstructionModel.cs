using System.Diagnostics;
using System.Numerics;
using Caiman.Core.Depr.Construction.Exceptions;

namespace Caiman.Core.Depr.Construction;

public class ConstructionModel : IConstructionModel
{
    // TODO переместить в Editor
    public List<Node> Nodes { get; set; } = [];
    public List<Element> Elements { get; set; } = [];

    private int RemoveElements(Node node) =>
        Elements.RemoveAll(element => element.StartNode == node || element.EndNode == node);

    private void NumberElements()
    {
        foreach (Element element in Elements)
        {
            element.Id = Elements.IndexOf(element);
        }
    }

    private void NumberNodes()
    {
        foreach (Node node in Nodes)
        {
            node.Id = Nodes.IndexOf(node);
        }
    }

    public void AddDeletedNode(Node node)
    {
        Nodes.Insert(node.Id, node);
        foreach (Element element in node.Elements)
        {
            AddDeletedElement(element, true);
        }

        if (node.Elements.Count > 0)
        {
            NumberElements();
        }

        NumberNodes();
    }

    public int AddNode(Vector2 position)
    {
        Node? node = Nodes.FirstOrDefault(node =>
            Math.Abs(node.Position.X - position.X) < Constants.Epsilon &&
            Math.Abs(node.Position.Y - position.Y) < Constants.Epsilon);
        if (node is not null)
        {
            throw new NodeAlreadyExistsException();
        }

        var newNode = new Node(position);
        Nodes.Add(
            newNode
        );
        NumberNodes();
        return newNode.Id;
    }

    public Node RemoveNode(int nodeId)
    {
        // TODO переделать на FirstOrDefault
        try
        {
            Node node = Nodes[nodeId];
            Nodes.Remove(node);
            int removedElements = RemoveElements(node);
            if (removedElements > 0)
            {
                NumberElements();
            }

            NumberNodes();
            return node;
        }
        catch (IndexOutOfRangeException)
        {
            throw new NodeNotFoundException();
        }
    }

    public int AddElement(int startNodeId, int endNodeId, double elasticity, double area)
    {
        if (startNodeId < 0 || startNodeId >= Nodes.Count
            || endNodeId < 0 || endNodeId >=
            Nodes.Count)
        {
            Debug.WriteLine("Invalid Id");
            throw new NodeNotFoundException();
        }

        if (startNodeId == endNodeId)
        {
            Debug.WriteLine("Start and end nodes are the same");
            throw new SameNodesException();
        }

        Node startNode = Nodes[startNodeId];
        Node endNode = Nodes[endNodeId];

        Element? element = Elements.FirstOrDefault(el =>
            (el.StartNode.Id == startNode.Id || el.StartNode.Id == endNode.Id) && (el.EndNode.Id == startNode.Id ||
                el.EndNode.Id == endNode.Id)
        );
        if (element is not null)
        {
            Debug.WriteLine("ElementEntity already exists");
            throw new ElementAlreadyExistsException();
        }

        var newElement = new Element(
            startNode,
            endNode,
            elasticity,
            area
        );
        Elements.Add(
            newElement
        );
        startNode.Elements.Add(newElement);
        endNode.Elements.Add(newElement);
        NumberElements();
        return newElement.Id;
    }

    public Element RemoveElement(int elementId)
    {
        // TODO переделать на FirstOrDefault
        try
        {
            Element element = Elements[elementId];
            Elements.Remove(element);
            element.StartNode.Elements.Remove(element);
            element.EndNode.Elements.Remove(element);
            NumberElements();
            return element;
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new ElementNotFoundException();
        }
    }

    public void AddDeletedElement(Element element, bool wasInNode = false)
    {
        Elements.Insert(element.Id, element);
        if (!wasInNode)
        {
            NumberElements();
        }
    }

    public void AddConstraint(int nodeId, bool x, bool y)
    {
        Node? node = Nodes.FirstOrDefault(n => n.Id == nodeId);
        if (node is null)
        {
            throw new NodeNotFoundException();
        }

        if (node.Constraint is not null)
        {
            throw new ConstraintAlreadyExistsException();
        }

        if (x == false && y == false)
        {
            throw new EmptyConstraintException();
        }

        node.Constraint = new Constraint(x, y);
    }

    public Constraint RemoveConstraint(int nodeId)
    {
        Node? node = Nodes.FirstOrDefault(n => n.Id == nodeId);
        if (node is null)
        {
            throw new NodeNotFoundException();
        }

        if (node.Constraint is null)
        {
            throw new ConstraintNotFoundException();
        }

        Constraint constraint = node.Constraint;
        node.Constraint = null;
        return constraint;
    }

    public void AddLoad(int nodeId, Vector2 value)
    {
        Node? node = Nodes.FirstOrDefault(n => n.Id == nodeId);
        if (node is null)
        {
            throw new NodeNotFoundException();
        }

        node.Loads.Add(new ConcentratedLoad(value));
    }

    public void RemoveLoad(int nodeId, Vector2 value)
    {
        Node? node = Nodes.FirstOrDefault(n => n.Id == nodeId);
        if (node is null)
        {
            throw new NodeNotFoundException();
        }

        ConcentratedLoad? load = node.Loads.FirstOrDefault(c => c.Value == value);
        if (load is null)
        {
            throw new LoadNotFoundException();
        }

        node.Loads.Remove(load);
    }
}
