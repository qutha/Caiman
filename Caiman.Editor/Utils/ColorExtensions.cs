using Raylib_cs;

namespace Caiman.Editor.Utils;

public static class ColorExtensions
{
    public static Color FromHsv(this Color color, double h, double s, double v, double a)
    {
        var hi = Convert.ToInt32(Math.Floor(h / 60)) % 6;
        var f = h / 60 - Math.Floor(h / 60);
        var p = v * (1 - s);
        var q = v * (1 - f * s);
        var t = v * (1 - (1 - f) * s);

        byte r, g, b;

        switch (hi)
        {
            case 0:
                r = Convert.ToByte(v * 255);
                g = Convert.ToByte(t * 255);
                b = Convert.ToByte(p * 255);
                break;
            case 1:
                r = Convert.ToByte(q * 255);
                g = Convert.ToByte(v * 255);
                b = Convert.ToByte(p * 255);
                break;
            case 2:
                r = Convert.ToByte(p * 255);
                g = Convert.ToByte(v * 255);
                b = Convert.ToByte(t * 255);
                break;
            case 3:
                r = Convert.ToByte(p * 255);
                g = Convert.ToByte(q * 255);
                b = Convert.ToByte(v * 255);
                break;
            case 4:
                r = Convert.ToByte(t * 255);
                g = Convert.ToByte(p * 255);
                b = Convert.ToByte(v * 255);
                break;
            default:
                r = Convert.ToByte(v * 255);
                g = Convert.ToByte(p * 255);
                b = Convert.ToByte(q * 255);
                break;
        }

        color.R = (byte)(r * 255);
        color.G = (byte)(g * 255);
        color.B = (byte)(b * 255);
        color.A = (byte)(a * 255);

        return color;
    }
}
