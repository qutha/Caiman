using System.Numerics;

namespace Caiman.Editor.Camera;

public class CameraModel
{
    public Vector2 Position { get; set; }
    public Vector2 Offset { get; set; }
    public float Zoom { get; set; }
}
