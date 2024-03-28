using System.Numerics;
using Caiman.Core.Depr.Construction;
using Caiman.Editor.Camera;
using Caiman.Editor.Commands;
using Caiman.Editor.Construction;
using Caiman.Editor.Explorer;
using Caiman.Editor.Interfaces;
using Caiman.Editor.Scene;
using Caiman.Editor.Utils;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;
using ImGuiStyle = Caiman.Editor.Utils.ImGuiStyle;

namespace Caiman.Editor;

public class Editor : IUnmanaged
{
    private readonly CameraController _cameraController;
    private readonly CommandManager _commandManager;
    private readonly ConstructionController _constructionController;
    private readonly ConstructionGraphicsView _constructionGraphicsView;
    private readonly ConstructionModel _constructionModel;
    private readonly ExplorerController _explorerController;

    private readonly QueueManager _queueManager = new();
    private readonly SceneBuffer _sceneBuffer;
    private readonly SceneController _sceneController;

    public Editor()
    {
        //
        //  Command Manager
        //
        _commandManager = new CommandManager();
        _queueManager.Add(_commandManager);

        //
        //  Scene
        //
        _sceneBuffer = new SceneBuffer();

        _constructionModel = new ConstructionModel();
        var node1 = new Node(Vector2.Zero);
        _constructionModel.Nodes.Add(node1);

        _constructionGraphicsView = new ConstructionGraphicsView(_constructionModel);
        _explorerController = new ExplorerController(_commandManager, _constructionModel);
        _constructionController = new ConstructionController(_commandManager, _constructionModel);


        _cameraController = new CameraController(_sceneBuffer);
        _queueManager.Add(_cameraController);
        _queueManager.Add(_constructionGraphicsView);
        _queueManager.Add(_constructionController);
        _queueManager.Add(_explorerController);


        _sceneController = new SceneController(_sceneBuffer, _cameraController);
        _queueManager.Add(_sceneController);

        // _queueManager.Add(new BorderRender(_sceneBuffer));
        _queueManager.Add(new AxisRender(_sceneBuffer));

        Init();
    }

    public void Init()
    {
        Raylib.InitWindow(1280, 720, "Editor");
        Raylib.SetWindowState(ConfigFlags.ResizableWindow);
        Raylib.SetTargetFPS(60);
        rlImGui.Setup(false, true);
        ImGuiStyle.DarkRed();
        _sceneBuffer.RenderTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    }


    public void Shutdown()
    {
        rlImGui.Shutdown();
        Raylib.CloseWindow();
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Update();
            Render();
        }

        Shutdown();
    }

    private void Update() => _queueManager.Update();

    private void Render()
    {
        Raylib.BeginTextureMode(_sceneBuffer.RenderTexture);
        Raylib.ClearBackground(Color.Gray);
        Raylib.BeginMode2D(_cameraController.Camera);
        _queueManager.RenderWorld();

        Raylib.EndMode2D();
        _queueManager.Render();

        Raylib.EndTextureMode();
        Raylib.BeginDrawing();
        RenderGui();
        Raylib.EndDrawing();
    }

    private void RenderGui()
    {
        rlImGui.Begin();
        ImGui.DockSpaceOverViewport();
        _queueManager.RenderGui();

        rlImGui.End();
    }
}
