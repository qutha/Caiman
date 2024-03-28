using Caiman.Core.Depr.Construction;
using Caiman.Editor.Commands;
using Caiman.Editor.Construction.Commands;
using Caiman.Editor.Interfaces;

namespace Caiman.Editor.Explorer;

public class ExplorerController : IGuiRender
{
    private readonly ICommandManager _commandManager;
    private readonly ConstructionModel _constructionModel;
    private readonly ExplorerView _view;

    public ExplorerController(
        ICommandManager commandManager,
        ConstructionModel constructionModel)
    {
        _commandManager = commandManager;
        _constructionModel = constructionModel;
        _view = new ExplorerView(_constructionModel);

        _view.RemoveNode += RemoveNode;
        _view.RemoveElement += RemoveElement;
    }

    public void RenderGui() => _view.RenderGui();

    private void RemoveElement(int elementId) =>
        _commandManager.Push(new RemoveElementCommand(_constructionModel, elementId));

    private void RemoveNode(int nodeId) => _commandManager.Push(new RemoveNodeCommand(_constructionModel, nodeId));
}
