using Caiman.Core.Depr.Construction;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class RemoveConstraintCommand(ConstructionModel constructionModel, int nodeId) : ICommand
{
    private Constraint? _deletedConstraint;

    public string Name => "Remove Constraint";

    public void Execute() => _deletedConstraint = constructionModel.RemoveConstraint(nodeId);

    public void Undo() => constructionModel.AddConstraint(nodeId, _deletedConstraint!.X, _deletedConstraint.Y);
}
