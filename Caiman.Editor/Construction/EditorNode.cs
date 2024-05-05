using System.Numerics;
using Caiman.Core.Construction;
using Caiman.Editor.Interfaces;

namespace Caiman.Editor.Construction;

public class EditorNode(Vector2 position) : IWorldRender
{
    public int Id { get; set; }
    public List<EditorElement> Elements { get; set; } = [];
    public List<ConcentratedLoad> Loads { get; set; } = [];
    public Constraint Constraint { get; set; } = new(false, false);

    public float X { get; set; } = position.X;

    public float Y { get; set; } = position.Y;

    #region IWorldRender Members

    public void RenderWorld() => throw new NotImplementedException();

    #endregion

    public override string ToString()
    {
        var x = Constraint.X ? "U" : "";
        var y = Constraint.Y ? "V" : "";
        var constraint = Constraint switch
        {
            { X: false, Y: false } => "Released",
            _ => $"{x}{y}",
        };

        return $"Node:\n\tId: {Id}\n\tPosition: {X}, {-Y}\n\t{constraint} Constraint";
    }
}
