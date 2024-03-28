using Caiman.Editor.Interfaces;
using Caiman.Editor.Scene;
using Raylib_cs;

namespace Caiman.Editor.Utils;

public class BorderRender : IRender
{
    private readonly SceneBuffer _sceneBuffer;

    public BorderRender(SceneBuffer sceneBuffer) => _sceneBuffer = sceneBuffer;

    public void Render()
    {
        Raylib.DrawRectangleLinesEx(new Rectangle(0, 0, _sceneBuffer.ViewportSize.X, _sceneBuffer.ViewportSize.Y), 5,
            Color.Black);
    }
}
