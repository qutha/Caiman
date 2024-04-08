using System.Text.Json.Serialization;

namespace Caiman.Core.Depr.Construction;

public class Element
{
    public int EndNodeId;
    public int StartNodeId;

    public Element(int startNodeId, int endNodeId, double elasticity, double area)
    {
        StartNodeId = startNodeId;
        EndNodeId = endNodeId;
        Elasticity = elasticity;
        Area = area;
    }

    public Element(Node startNode, Node endNode, double elasticity, double area)
    {
        StartNode = startNode;
        EndNode = endNode;
        StartNodeId = startNode.Id;
        EndNodeId = endNode.Id;
        Elasticity = elasticity;
        Area = area;
    }

    public int Id { get; set; }

    [JsonIgnore] public Node StartNode { get; set; }

    [JsonIgnore] public Node EndNode { get; set; }

    public double Elasticity { get; set; }
    public double Area { get; set; }

    [JsonIgnore] public double Stiffness => Elasticity * Area / Length;

    [JsonIgnore] public double Dx => EndNode.Position.X - StartNode.Position.X;

    [JsonIgnore] public double Dy => EndNode.Position.Y - StartNode.Position.Y;

    [JsonIgnore] public double Length => Math.Sqrt(Dx * Dx + Dy * Dy);

    [JsonIgnore] public double Sin => Dy / Length;

    [JsonIgnore] public double Cos => Dx / Length;

    public override string ToString() =>
        $"Element:\n\tId: {Id}, Start Node: {StartNode.Id}, End Node: {EndNode.Id}, Elasticity: {Elasticity}, Area: {Area}";
}
