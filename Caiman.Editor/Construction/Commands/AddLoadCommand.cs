using System.Numerics;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class AddLoadCommand(EditorConstruction construction, int nodeId, Vector2 value) : ICommand
{
    #region ICommand Members

    public string Name => "Add Load";

    public void Execute() => construction.AddLoad(nodeId, value);

    public void Undo() => construction.RemoveLoad(nodeId, value);

    #endregion
}
