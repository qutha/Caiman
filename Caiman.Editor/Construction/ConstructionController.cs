using System.Numerics;
using Caiman.Editor.Camera;
using Caiman.Editor.Commands;
using Caiman.Editor.Construction.Commands;
using Caiman.Editor.Interfaces;

namespace Caiman.Editor.Construction;

public class ConstructionController : IGuiRender, IWorldRender
{
    private readonly CameraController _cameraController;
    private readonly ICommandManager _commandManager;
    private readonly EditorConstruction _editorConstruction;
    private readonly ConstructionGuiView _guiView;


    public ConstructionController(
        ICommandManager commandManager,
        EditorConstruction editorConstruction,
        CameraController cameraController)
    {
        _editorConstruction = editorConstruction;
        _commandManager = commandManager;
        _cameraController = cameraController;
        _cameraController.ZoomChanged += _editorConstruction.ChangeZoom;
        _guiView = new ConstructionGuiView(editorConstruction);
        _guiView.AddNode += AddNode;
        _guiView.RemoveNode += RemoveNode;
        _guiView.AddElement += AddElement;
        _guiView.RemoveElement += RemoveElement;
        _guiView.AddConstraint += AddConstraint;
        _guiView.RemoveConstraint += RemoveConstraint;
        _guiView.AddLoad += AddLoad;
        _guiView.RemoveLoad += RemoveLoad;
    }

    #region IGuiRender Members

    public void RenderGui() => _guiView.RenderGui();

    #endregion

    #region IWorldRender Members

    public void RenderWorld() => _editorConstruction.RenderWorld();

    #endregion

    private void RemoveLoad(int nodeId, int loadId) =>
        _commandManager.Push(new RemoveLoadCommand(_editorConstruction, nodeId, loadId));

    private void AddLoad(int nodeId, Vector2 value) =>
        _commandManager.Push(new AddLoadCommand(_editorConstruction, nodeId, value));

    private void RemoveConstraint(int nodeId) =>
        _commandManager.Push(new RemoveConstraintCommand(_editorConstruction, nodeId));

    private void AddConstraint(int nodeId, bool x, bool y) =>
        _commandManager.Push(new AddConstraintCommand(_editorConstruction, nodeId, x, y));

    private void AddNode(Vector2 position) => _commandManager.Push(new AddNodeCommand(_editorConstruction, position));

    private void RemoveNode(int nodeId) => _commandManager.Push(new RemoveNodeCommand(_editorConstruction, nodeId));

    private void AddElement((int startNodeId, int endNodeId, double elasticity, double area) element) =>
        _commandManager.Push(new AddElementCommand(_editorConstruction, element.startNodeId, element.endNodeId,
            element.elasticity, element.area));

    private void RemoveElement(int elementId) =>
        _commandManager.Push(new RemoveElementCommand(_editorConstruction, elementId));
}
