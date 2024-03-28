using Caiman.Core.Depr.Construction;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class AddConstraintCommand : ICommand
{
    private readonly ConstructionModel _constructionModel;
    private readonly int _nodeId;
    private readonly bool _x;
    private readonly bool _y;

    public AddConstraintCommand(ConstructionModel constructionModel, int nodeId, bool x, bool y)
    {
        _constructionModel = constructionModel;
        _nodeId = nodeId;
        _x = x;
        _y = y;
    }

    public string Name => "Add Constraint";

    public void Execute() => _constructionModel.AddConstraint(_nodeId, _x, _y);

    public void Undo() => _constructionModel.RemoveConstraint(_nodeId);
}
