using Caiman.Core.Construction.Exceptions;

namespace Caiman.Core.Construction;

public class ConstructionBuilder
{
    private readonly Construction _construction = new();

    public ConstructionBuilder AddNode(Node node)
    {
        _construction.Nodes.Add(node);

        return this;
    }

    public ConstructionBuilder AddElement(Element element)
    {
        Node? startNode = _construction.Nodes.FirstOrDefault(n =>
            Math.Abs(n.X - element.StartNode.X) < Constants.Epsilon &&
            Math.Abs(n.Y - element.StartNode.Y) < Constants.Epsilon);
        Node? endNode = _construction.Nodes.FirstOrDefault(n =>
            Math.Abs(n.X - element.EndNode.X) < Constants.Epsilon &&
            Math.Abs(n.Y - element.EndNode.Y) < Constants.Epsilon);
        if (startNode is null || endNode is null)
        {
            throw new NodeNotFoundException();
        }

        _construction.Elements.Add(element);
        return this;
    }


    public ConstructionBuilder AddLoad(Node node, ConcentratedLoad load)
    {
        Node? constructionNode = _construction.Nodes.FirstOrDefault(n =>
            Math.Abs(n.X - node.X) < Constants.Epsilon && Math.Abs(n.Y - node.Y) < Constants.Epsilon);
        if (constructionNode is null)
        {
            throw new NodeNotFoundException();
        }

        constructionNode.LoadList.Add(load);
        return this;
    }

    public ConstructionBuilder AddConstraint(Node node, Constraint constraint)
    {
        Node? constructionNode = _construction.Nodes.FirstOrDefault(n =>
            Math.Abs(n.X - node.X) < Constants.Epsilon && Math.Abs(n.Y - node.Y) < Constants.Epsilon);
        if (constructionNode is null)
        {
            throw new NodeNotFoundException();
        }

        constructionNode.Constraint = constraint;
        return this;
    }

    public Construction Build() => _construction;
}
