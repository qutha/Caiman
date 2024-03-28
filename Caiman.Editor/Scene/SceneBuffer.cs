using System.Numerics;
using Raylib_cs;

namespace Caiman.Editor.Scene;

public class SceneBuffer
{
    public RenderTexture2D RenderTexture { get; set; }
    public Vector2 ViewportSize { get; set; } = new(800, 600);
}
