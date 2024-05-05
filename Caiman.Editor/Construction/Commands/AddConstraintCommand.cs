using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class AddConstraintCommand : ICommand
{
    private readonly EditorConstruction _construction;
    private readonly int _nodeId;
    private readonly bool _x;
    private readonly bool _y;

    public AddConstraintCommand(EditorConstruction construction, int nodeId, bool x, bool y)
    {
        _construction = construction;
        _nodeId = nodeId;
        _x = x;
        _y = y;
    }

    #region ICommand Members

    public string Name => "Add Constraint";

    public void Execute() => _construction.AddConstraint(_nodeId, _x, _y);

    public void Undo() => _construction.RemoveConstraint(_nodeId);

    #endregion
}
