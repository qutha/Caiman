using System.Numerics;
using Caiman.Core.Depr.Construction;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class AddNodeCommand(ConstructionModel constructionModel, Vector2 position) : ICommand
{
    private int _addedNodeId;

    public string Name => "Add Node";

    public void Execute() => _addedNodeId = constructionModel.AddNode(position);

    public void Undo() => constructionModel.RemoveNode(_addedNodeId);
}
