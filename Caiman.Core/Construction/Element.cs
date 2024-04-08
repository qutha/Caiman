namespace Caiman.Core.Construction;

public class Element(Node startNode, Node endNode, double elasticity, double area)
{
    public Node StartNode { get; set; } = startNode;

    public Node EndNode { get; set; } = endNode;

    /// <summary>
    ///     Модуль упругости, в кг/см^2
    /// </summary>
    public double Elasticity { get; set; } = elasticity;

    /// <summary>
    ///     Площадь поперечного сечения, в см^2
    /// </summary>
    public double Area { get; set; } = area;

    public double Stiffness => Elasticity * Area / Length;

    public double Dx => EndNode.X - StartNode.X;

    public double Dy => EndNode.Y - StartNode.Y;

    public double Length => Math.Sqrt(Dx * Dx + Dy * Dy);

    public double Sin => Dy / Length;

    public double Cos => Dx / Length;

    // public override string ToString() =>
    //     $"Element:\n\tId: {Id}, Start Node: {StartNode.Id}, End Node: {EndNode.Id}, Elasticity: {Elasticity}, Area: {Area}";
}
