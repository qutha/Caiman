namespace Caiman.CoreDepr.RodSystemSolver;

// TODO как структуры хранятся в классах
public class Element
{
    public Element(Node startNode, Node endNode, ElementProperties properties)
    {
        StartNode = startNode;
        EndNode = endNode;
        Properties = properties;
    }

    public Node StartNode { get; set; }
    public Node EndNode { get; }
    public ElementProperties Properties { get; set; }

    public double Dx => EndNode.X - StartNode.X;
    public double Dy => EndNode.Y - StartNode.Y;
    public double Length => Math.Sqrt(Dx * Dx + Dy * Dy);
    public double Sin => Dy / Length;
    public double Cos => Dx / Length;
    public double Stiffness => Properties.Elasticity * Properties.CrossSectionalArea / Length;
}
