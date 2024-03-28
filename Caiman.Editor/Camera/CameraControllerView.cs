using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Camera;

public class CameraControllerView : IGuiRender
{
    private readonly CameraModel _camera;
    private float _zoomFactor;

    public CameraControllerView(CameraModel camera, float zoomFactor)
    {
        _camera = camera;
        _zoomFactor = zoomFactor;
    }

    public void RenderGui()
    {
        ImGui.Begin("Camera");

        if (ImGui.SliderFloat("Zoom Factor", ref _zoomFactor, 0.05f, 0.5f))
        {
            ZoomFactorChanged?.Invoke(_zoomFactor);
        }

        ImGui.Text($"Camera Pos: {_camera.Position}");
        ImGui.Text($"Camera Zoom: {_camera.Zoom}");

        ImGui.End();
    }

    public event Action<float>? ZoomFactorChanged;
}
