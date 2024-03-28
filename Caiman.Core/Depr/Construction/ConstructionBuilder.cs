using Caiman.Core.Depr.Construction.Exceptions;

namespace Caiman.Core.Depr.Construction;

public class ConstructionBuilder
{
    private readonly ConstructionModel _construction = new();

    public ConstructionBuilder AddNode(Node node)
    {
        _construction.AddNode(node.Position);

        return this;
    }

    public ConstructionBuilder AddElement(Element element)
    {
        var startNode = _construction.Nodes.FirstOrDefault(n => n.Position == element.StartNode.Position);
        var endNode = _construction.Nodes.FirstOrDefault(n => n.Position == element.EndNode.Position);
        if (startNode is null || endNode is null)
        {
            throw new NodeNotFoundException();
        }

        _construction.AddElement(startNode.Id, endNode.Id, element.Elasticity, element.Area);
        return this;
    }


    public ConstructionBuilder AddLoad(Node node, ConcentratedLoad load)
    {
        var constructionNode = _construction.Nodes.FirstOrDefault(n => n.Position == node.Position);
        if (constructionNode is null)
        {
            throw new NodeNotFoundException();
        }

        _construction.AddLoad(constructionNode.Id, load.Value);
        return this;
    }

    public ConstructionBuilder AddConstraint(Node node, Constraint constraint)
    {
        var constructionNode = _construction.Nodes.FirstOrDefault(n => n.Position == node.Position);
        if (constructionNode is null)
        {
            throw new NodeNotFoundException();
        }

        _construction.AddConstraint(constructionNode.Id, constraint.X, constraint.Y);

        return this;
    }

    public IConstructionModel Build() => _construction;
}
