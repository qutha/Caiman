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
    private readonly float _minZoom = 0.05f;
    private readonly SceneBuffer _sceneBuffer;
    private float _maxZoom = 0.05f;
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
        _cameraControllerView.Center += CenterCamera;
        _cameraControllerView.ResetZoom += ResetZoom;
    }

    public float ZoomFactor { get; set; } = 0.125f;

    #region IGuiRender Members

    public void RenderGui() => _cameraControllerView.RenderGui();

    #endregion

    #region IUpdate Members

    public void Update()
    {
        Camera.Target = _cameraModel.Position;
        Camera.Zoom = _cameraModel.Zoom;
        Camera.Offset = _cameraModel.Offset;
    }

    #endregion

    private void ResetZoom() => _cameraModel.Zoom = 2;

    public void CenterCamera()
    {
        _cameraModel.Position = new Vector2(0, 0);
        _cameraModel.Offset = _sceneBuffer.ViewportSize / 2f;
    }

    private void ZoomFactorChanged(float zoomFactor) => ZoomFactor = zoomFactor;


    public void DragCamera(Vector2 vector)
    {
        vector *= -1.0f / _cameraModel.Zoom;
        _cameraModel.Position += vector;
    }

    public void Zoom(Vector2 sceneMousePosition, float scroll)
    {
        var worldMousePosition = Transform.GetScreenToWorld(sceneMousePosition, Camera);
        _cameraModel.Offset = sceneMousePosition;
        // Y is inverted
        _cameraModel.Position = worldMousePosition with { Y = -worldMousePosition.Y };
        _cameraModel.Zoom += ZoomFactor * scroll;
        if (_cameraModel.Zoom < _minZoom)
        {
            _cameraModel.Zoom = _minZoom;
        }

        ZoomChanged?.Invoke(_cameraModel.Zoom);
    }

    public event Action<float>? ZoomChanged;
}
