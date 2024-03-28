namespace Caiman.CoreDepr.RodSystemSolver;

public class Node(double x, double y) : IEquatable<Node>
{
    public int Id { get; set; }
    public double X { get; } = x;
    public double Y { get; } = y;

    public bool Equals(Node? other)
    {
        if (other is null)
        {
            return false;
        }

        return Math.Abs(X - other.X) < Constants.Epsilon && Math.Abs(Y - other.Y) < Constants.Epsilon;
    }

    public override bool Equals(object? obj) => Equals(obj as Node);

    public override int GetHashCode() =>
        HashCode.Combine(X, Y);
}
