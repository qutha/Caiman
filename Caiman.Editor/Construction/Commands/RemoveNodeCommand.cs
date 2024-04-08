using Caiman.Core.Depr.Construction;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class RemoveNodeCommand : ICommand
{
    private readonly ConstructionModel _constructionModel;
    private readonly int _nodeId;
    private Node? _deletedNode;

    public RemoveNodeCommand(ConstructionModel constructionModel, int nodeId)
    {
        _constructionModel = constructionModel;
        _nodeId = nodeId;
    }

    public string Name => "Remove Node";

    public void Execute() => _deletedNode = _constructionModel.RemoveNode(_nodeId);

    public void Undo()
    {
        if (_deletedNode is null)
        {
            return;
        }

        _constructionModel.AddDeletedNode(_deletedNode);
    }
}
