using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class RemoveNodeCommand : ICommand
{
    private readonly EditorConstruction _construction;
    private readonly int _nodeId;
    private EditorNode? _deletedNode;

    public RemoveNodeCommand(EditorConstruction construction, int nodeId)
    {
        _construction = construction;
        _nodeId = nodeId;
    }

    #region ICommand Members

    public string Name => "Remove Node";

    public void Execute() => _deletedNode = _construction.RemoveNode(_nodeId);

    public void Undo()
    {
        if (_deletedNode is null)
        {
            return;
        }

        _construction.AddDeletedNode(_deletedNode);
    }

    #endregion
}
