using Caiman.Editor.Camera;
using Caiman.Editor.Interfaces;
using Caiman.Editor.Scene;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;
using ImGuiStyle = Caiman.Editor.Utils.ImGuiStyle;

namespace Caiman.Editor;

public class Editor : IUnmanaged
{
    private readonly Color _backgroundColor = new(50, 50, 50, 239);
    private readonly CameraController _cameraController;
    private readonly QueueManager _queueManager;
    private readonly SceneBuffer _sceneBuffer;

    public Editor(
        QueueManager queueManager,
        SceneBuffer sceneBuffer,
        CameraController cameraController
    )
    {
        _queueManager = queueManager;
        _sceneBuffer = sceneBuffer;
        _cameraController = cameraController;

        Init();
    }

    #region IUnmanaged Members

    public void Init()
    {
        Raylib.InitWindow(1280, 720, "Caiman");
        Raylib.SetWindowState(ConfigFlags.ResizableWindow);
#if DEBUG
        Raylib.SetTargetFPS(165);
#else
        Raylib.SetTargetFPS(60);
#endif

        rlImGui.Setup(false, true);
        ImGuiStyle.DarkRed();
        _sceneBuffer.RenderTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    }


    public void Shutdown()
    {
        rlImGui.Shutdown();
        Raylib.CloseWindow();
    }

    #endregion

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
        Raylib.ClearBackground(_backgroundColor);
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
