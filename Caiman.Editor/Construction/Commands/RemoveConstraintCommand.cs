using Caiman.Core.Construction;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class RemoveConstraintCommand(EditorConstruction construction, int nodeId) : ICommand
{
    private Constraint? _deletedConstraint;

    #region ICommand Members

    public string Name => "Remove Constraint";

    public void Execute() => _deletedConstraint = construction.RemoveConstraint(nodeId);

    public void Undo() => construction.AddConstraint(nodeId, _deletedConstraint!.X, _deletedConstraint.Y);

    #endregion
}
