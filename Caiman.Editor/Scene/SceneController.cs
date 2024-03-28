using Caiman.Editor.Camera;
using Caiman.Editor.Interfaces;
using Raylib_cs;

namespace Caiman.Editor.Scene;

public class SceneController : IGuiRender, IUpdate
{
    private readonly CameraController _cameraController;
    private readonly SceneBuffer _sceneBuffer;
    private readonly SceneView _view;

    public SceneController(SceneBuffer sceneBuffer, CameraController cameraController)
    {
        _sceneBuffer = sceneBuffer;
        _cameraController = cameraController;
        _view = new SceneView(_sceneBuffer);
        _view.Resized += size => _sceneBuffer.ViewportSize = size;
        _view.MiddleButtonDragged += delta => _cameraController.DragCamera(delta);
        _view.MouseScroll += (delta, scroll) => _cameraController.Zoom(delta, scroll);
        _view.DoubleMiddleMouseButtonClicked += () => _cameraController.CenterCamera();
    }

    public void RenderGui()
    {
        _view.RenderGui();
    }

    public void Update()
    {
        if (Raylib.IsWindowResized())
        {
            Raylib.UnloadRenderTexture(_sceneBuffer.RenderTexture);
            _sceneBuffer.RenderTexture =
                Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        }
    }
}
