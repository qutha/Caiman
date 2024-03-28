namespace Caiman.CoreDepr.RodSystemSolver;

public class NodeReference(Node node)
{
    public int Id { get; set; }
    public Position? Position { get; set; }
    public Node Node { get; set; } = node;
    public IList<ConcentratedLoad> Loads { get; set; } = new List<ConcentratedLoad>();
    public IList<Element> Elements { get; set; } = new List<Element>();
    public Constraint? Constraint { get; set; }
}
