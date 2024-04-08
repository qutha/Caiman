namespace Caiman.Core.Construction;

public class Node(double x, double y)
{
    public List<ConcentratedLoad> LoadList { get; set; } = [];
    public Constraint? Constraint { get; set; }

    /// <summary>
    ///     Координата X узла, в метрах
    /// </summary>
    public double X { get; set; } = x;

    /// <summary>
    ///     Координата Y узла, в метрах
    /// </summary>
    public double Y { get; set; } = y;

    // public override string ToString() => Constraint is null
    //     ? $"Node:\n\tId: {Id}, Position: {Position}"
    //     : $"Node:\n\tId: {Id}, Position: {Position} with ({Constraint.X}:{Constraint.Y}) constraint";
}
