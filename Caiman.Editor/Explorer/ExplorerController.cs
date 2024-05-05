using Caiman.Editor.Commands;
using Caiman.Editor.Construction;
using Caiman.Editor.Construction.Commands;
using Caiman.Editor.Interfaces;

namespace Caiman.Editor.Explorer;

public class ExplorerController : IGuiRender
{
    private readonly ICommandManager _commandManager;
    private readonly EditorConstruction _editorConstruction;
    private readonly ExplorerView _view;

    public ExplorerController(
        ICommandManager commandManager,
        EditorConstruction editorConstruction)
    {
        _commandManager = commandManager;
        _editorConstruction = editorConstruction;
        _view = new ExplorerView(_editorConstruction);

        _view.RemoveNode += RemoveNode;
        _view.RemoveElement += RemoveElement;
        _view.RemoveLoad += RemoveLoad;
    }

    #region IGuiRender Members

    public void RenderGui() => _view.RenderGui();

    #endregion

    private void RemoveLoad(int nodeId, int loadId) =>
        _commandManager.Push(new RemoveLoadCommand(_editorConstruction, nodeId, loadId));

    private void RemoveElement(int elementId) =>
        _commandManager.Push(new RemoveElementCommand(_editorConstruction, elementId));

    private void RemoveNode(int nodeId) => _commandManager.Push(new RemoveNodeCommand(_editorConstruction, nodeId));
}
