using System.Numerics;
using Raylib_cs;

namespace Caiman.Editor.Utils;

public static class Transform
{
    public static Vector2 GetScreenToWorld(Vector2 mouseScreenPos, Camera2D camera)
    {
        float x = (camera.Offset.X - mouseScreenPos.X) / camera.Zoom - camera.Target.X;
        x = -x;
        float y = (camera.Offset.Y - mouseScreenPos.Y) / camera.Zoom - camera.Target.Y;

        var worldMousePos = new Vector2(x, y);
        return worldMousePos;
    }
}
