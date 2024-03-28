using System.Numerics;
using Caiman.Core.Depr.Construction;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class AddLoadCommand(ConstructionModel constructionModel, int nodeId, Vector2 value) : ICommand
{
    public string Name => "Add Load";

    public void Execute() => constructionModel.AddLoad(nodeId, value);

    public void Undo() => constructionModel.RemoveLoad(nodeId, value);
}
