using System.Numerics;
using Caiman.Editor.Interfaces;
using Caiman.Editor.Scene;
using Raylib_cs;
using Transform = Caiman.Editor.Utils.Transform;

namespace Caiman.Editor.Camera;

public class CameraController : IUpdate, IGuiRender
{
    private readonly CameraControllerView _cameraControllerView;
    private readonly CameraModel _cameraModel;
    private readonly SceneBuffer _sceneBuffer;
    public Camera2D Camera;

    public CameraController(SceneBuffer sceneBuffer)
    {
        _sceneBuffer = sceneBuffer;
        Camera = new Camera2D
        {
            Target = new Vector2(0, 0),
            Zoom = 1.0f,
            Offset = _sceneBuffer.ViewportSize / 2f,
        };

        _cameraModel = new CameraModel
        {
            Position = Camera.Target,
            Offset = Camera.Offset,
            Zoom = Camera.Zoom,
        };

        _cameraControllerView = new CameraControllerView(_cameraModel, ZoomFactor);
        _cameraControllerView.ZoomFactorChanged += ZoomFactorChanged;
    }

    public float ZoomFactor { get; set; } = 0.125f;

    public void RenderGui()
    {
        _cameraControllerView.RenderGui();
    }

    public void Update()
    {
        Camera.Target = _cameraModel.Position;
        Camera.Zoom = _cameraModel.Zoom;
        Camera.Offset = _cameraModel.Offset;
    }

    public void CenterCamera()
    {
        _cameraModel.Position = new Vector2(0, 0);
        _cameraModel.Offset = _sceneBuffer.ViewportSize / 2f;
    }

    private void ZoomFactorChanged(float zoomFactor)
    {
        ZoomFactor = zoomFactor;
    }


    public void DragCamera(Vector2 vector)
    {
        vector *= -1.0f / _cameraModel.Zoom;
        _cameraModel.Position += vector;
    }

    public void Zoom(Vector2 sceneMousePosition, float scroll)
    {
        Vector2 worldMousePosition = Transform.GetScreenToWorld(sceneMousePosition, Camera);
        _cameraModel.Offset = sceneMousePosition;
        // Y is inverted
        _cameraModel.Position = worldMousePosition with { Y = -worldMousePosition.Y };
        _cameraModel.Zoom += ZoomFactor * scroll;
        if (_cameraModel.Zoom < ZoomFactor)
        {
            _cameraModel.Zoom = ZoomFactor;
        }
    }
}
