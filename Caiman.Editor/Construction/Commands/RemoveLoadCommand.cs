using System.Numerics;
using Caiman.Core.Construction;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class RemoveLoadCommand(EditorConstruction construction, int nodeId, int loadId) : ICommand
{
    private ConcentratedLoad _deletedLoad = null!;

    #region ICommand Members

    public string Name => "Remove Load";

    public void Execute() => _deletedLoad = construction.RemoveLoad(nodeId, loadId);

    public void Undo() => construction.AddLoad(nodeId, new Vector2((float)_deletedLoad.X, (float)_deletedLoad.Y));

    #endregion
}
