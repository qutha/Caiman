using Caiman.Core.Construction.Exceptions;

namespace Caiman.Core.Construction;

public class ConstructionBuilder
{
    private readonly ConstructionEntity _construction = new();

    public ConstructionBuilder AddNode(NodeEntity nodeEntity)
    {
        _construction.Nodes.Add(nodeEntity);

        return this;
    }

    public ConstructionBuilder AddElement(ElementEntity elementEntity)
    {
        NodeEntity? startNode = _construction.Nodes.FirstOrDefault(n =>
            Math.Abs(n.X - elementEntity.StartNodeEntity.X) < Constants.Epsilon &&
            Math.Abs(n.Y - elementEntity.StartNodeEntity.Y) < Constants.Epsilon);
        NodeEntity? endNode = _construction.Nodes.FirstOrDefault(n =>
            Math.Abs(n.X - elementEntity.EndNodeEntity.X) < Constants.Epsilon &&
            Math.Abs(n.Y - elementEntity.EndNodeEntity.Y) < Constants.Epsilon);
        if (startNode is null || endNode is null)
        {
            throw new NodeNotFoundException();
        }

        _construction.Elements.Add(elementEntity);
        return this;
    }


    public ConstructionBuilder AddLoad(NodeEntity nodeEntity, ConcentratedLoad load)
    {
        NodeEntity? constructionNode = _construction.Nodes.FirstOrDefault(n =>
            Math.Abs(n.X - nodeEntity.X) < Constants.Epsilon && Math.Abs(n.Y - nodeEntity.Y) < Constants.Epsilon);
        if (constructionNode is null)
        {
            throw new NodeNotFoundException();
        }

        constructionNode.LoadList.Add(load);
        return this;
    }

    public ConstructionBuilder AddConstraint(NodeEntity nodeEntity, Constraint constraint)
    {
        NodeEntity? constructionNode = _construction.Nodes.FirstOrDefault(n =>
            Math.Abs(n.X - nodeEntity.X) < Constants.Epsilon && Math.Abs(n.Y - nodeEntity.Y) < Constants.Epsilon);
        if (constructionNode is null)
        {
            throw new NodeNotFoundException();
        }

        constructionNode.Constraint = constraint;
        return this;
    }

    public ConstructionEntity Build() => _construction;
}
