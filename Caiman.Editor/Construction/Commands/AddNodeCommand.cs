using System.Numerics;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class AddNodeCommand(EditorConstruction construction, Vector2 position) : ICommand
{
    private int _addedNodeId;
    private readonly Vector2 _position = position with { Y = -position.Y };

    #region ICommand Members

    public string Name => "Add Node";

    public void Execute() => _addedNodeId = construction.AddNode(_position);

    public void Undo() => construction.RemoveNode(_addedNodeId);

    #endregion
}
