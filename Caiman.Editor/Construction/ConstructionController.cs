using System.Numerics;
using Caiman.Core.Depr.Construction;
using Caiman.Editor.Commands;
using Caiman.Editor.Construction.Commands;
using Caiman.Editor.Interfaces;

namespace Caiman.Editor.Construction;

public class ConstructionController : IGuiRender, IWorldRender
{
    private readonly CommandManager _commandManager;
    private readonly ConstructionModel _constructionModel;
    private readonly ConstructionGraphicsView _graphicsGraphicsView;
    private readonly ConstructionGuiView _guiView;


    public ConstructionController(
        CommandManager commandManager,
        ConstructionModel constructionModel)
    {
        _commandManager = commandManager;
        _graphicsGraphicsView = new ConstructionGraphicsView(constructionModel);
        _guiView = new ConstructionGuiView(constructionModel);
        _guiView.AddNode += AddNode;
        _guiView.RemoveNode += RemoveNode;
        _guiView.AddElement += AddElement;
        _guiView.RemoveElement += RemoveElement;
        _guiView.AddConstraint += AddConstraint;
        _guiView.RemoveConstraint += RemoveConstraint;
        _guiView.AddLoad += AddLoad;
        _guiView.RemoveLoad += RemoveLoad;


        _constructionModel = constructionModel;
    }

    public void RenderGui() => _guiView.RenderGui();

    public void RenderWorld() => _graphicsGraphicsView.RenderWorld();

    private void RemoveLoad(int nodeId, Vector2 value)
    {
        // _commandManager.Push(new RemoveLoadCommand(_constructionModel, nodeId, value));
    }

    private void AddLoad(int nodeId, Vector2 value) =>
        _commandManager.Push(new AddLoadCommand(_constructionModel, nodeId, value));

    private void RemoveConstraint(int nodeId) =>
        _commandManager.Push(new RemoveConstraintCommand(_constructionModel, nodeId));

    private void AddConstraint(int nodeId, bool x, bool y) =>
        _commandManager.Push(new AddConstraintCommand(_constructionModel, nodeId, x, y));

    private void AddNode(Vector2 position) => _commandManager.Push(new AddNodeCommand(_constructionModel, position));

    private void RemoveNode(int nodeId) => _commandManager.Push(new RemoveNodeCommand(_constructionModel, nodeId));

    private void AddElement((int startNodeId, int endNodeId, double elasticity, double area) element) =>
        _commandManager.Push(new AddElementCommand(_constructionModel, element.startNodeId, element.endNodeId,
            element.elasticity, element.area));

    private void RemoveElement(int elementId) =>
        _commandManager.Push(new RemoveElementCommand(_constructionModel, elementId));
}
