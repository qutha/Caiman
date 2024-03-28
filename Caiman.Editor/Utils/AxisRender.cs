using System.Numerics;
using Caiman.Editor.Interfaces;
using Caiman.Editor.Scene;
using Raylib_cs;

namespace Caiman.Editor.Utils;

public class AxisRender : IRender
{
    private readonly SceneBuffer _sceneBuffer;

    public AxisRender(SceneBuffer sceneBuffer) => _sceneBuffer = sceneBuffer;

    public void Render()
    {
        const float margin = 10;
        const float length = 100;
        const float thickness = 2;
        // X axis
        Raylib.DrawLineEx(
            new Vector2(margin, _sceneBuffer.ViewportSize.Y - margin),
            new Vector2(margin + length, _sceneBuffer.ViewportSize.Y - margin),
            thickness, Color.Red);

        // Y axis
        Raylib.DrawLineEx(
            new Vector2(margin, _sceneBuffer.ViewportSize.Y - margin),
            new Vector2(margin, _sceneBuffer.ViewportSize.Y - margin - length),
            thickness, Color.Green);
    }
}
