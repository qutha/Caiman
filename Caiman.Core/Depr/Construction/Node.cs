using System.Numerics;

namespace Caiman.Core.Depr.Construction;

public class Node(Vector2 position)
{
    public int Id { get; set; }
    public List<Element> Elements { get; set; } = [];
    public List<int> ElementIds { get; set; } = [];
    public List<ConcentratedLoad> Loads { get; set; } = [];
    public Constraint? Constraint { get; set; }
    public Vector2 Position { get; set; } = position;

    public override string ToString() => Constraint is null
        ? $"NodeEntity:\n\tId: {Id}, Position: {Position}"
        : $"NodeEntity:\n\tId: {Id}, Position: {Position} with ({Constraint.X}:{Constraint.Y}) constraint";
}
