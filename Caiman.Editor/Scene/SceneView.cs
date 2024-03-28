using System.Numerics;
using Caiman.Editor.Interfaces;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

namespace Caiman.Editor.Scene;

public class SceneView : IGuiRender
{
    private readonly SceneBuffer _sceneBuffer;

    public SceneView(SceneBuffer sceneBuffer) => _sceneBuffer = sceneBuffer;

    public void RenderGui()
    {
        ImGui.SetNextWindowSizeConstraints(new Vector2(100, 100),
            new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));
        if (ImGui.Begin("View"))
        {
            Vector2 contentRegionAvail = ImGui.GetContentRegionAvail();
            if (contentRegionAvail != _sceneBuffer.ViewportSize)
            {
                Resized?.Invoke(contentRegionAvail);
            }

            if (ImGui.IsWindowHovered())
            {
                if (ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Middle))
                {
                    DoubleMiddleMouseButtonClicked?.Invoke();
                }

                Vector2 relativeMousePosition = ImGui.GetMousePos() - ImGui.GetCursorScreenPos();
                if (ImGui.GetIO().MouseWheel != 0)
                {
                    MouseScroll?.Invoke(relativeMousePosition, ImGui.GetIO().MouseWheel);
                }

                if (ImGui.IsMouseDown(ImGuiMouseButton.Middle))
                {
                    MiddleButtonDragged?.Invoke(Raylib.GetMouseDelta());
                }
            }


            var destWidth = (int)contentRegionAvail.X;
            var destHeight = (int)contentRegionAvail.Y;
            rlImGui.ImageRect(_sceneBuffer.RenderTexture.Texture, destWidth, destHeight,
                new Rectangle(0.0f, 0.0f, contentRegionAvail.X, -contentRegionAvail.Y));
        }

        ImGui.End();
    }

    public event Action<Vector2>? Resized;
    public event Action<Vector2>? MiddleButtonDragged;
    public event Action<Vector2, float>? MouseScroll;
    public event Action? DoubleMiddleMouseButtonClicked;
}
