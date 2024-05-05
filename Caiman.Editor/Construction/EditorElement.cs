namespace Caiman.Editor.Construction;

public class EditorElement(EditorNode startNode, EditorNode endNode, double elasticity, double area)
{
    public int Id { get; set; }

    public EditorNode StartNode { get; set; } = startNode;

    public EditorNode EndNode { get; set; } = endNode;

    public double Elasticity { get; set; } = elasticity;
    public double Area { get; set; } = area;

    public double Stiffness => Elasticity * Area / Length;

    public double Dx => EndNode.X - StartNode.X;

    public double Dy => EndNode.Y - StartNode.Y;

    public double Length => Math.Sqrt(Dx * Dx + Dy * Dy);

    public double Sin => Dy / Length;

    public double Cos => Dx / Length;

    public override string ToString() =>
        $"Element:\n\tId: {Id}\n\tStart Node: {StartNode.Id}\n\tEnd Node: {EndNode.Id}\n\tElasticity: {Elasticity}\n\tArea: {Area}";
}
