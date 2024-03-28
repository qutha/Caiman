namespace Caiman.Core.Construction;

public class ConcentratedLoad(double x, double y)
{
    /// <summary>
    ///     Значение компоненты X, в кг.
    /// </summary>
    public double X { get; set; } = x;

    /// <summary>
    ///     Значение компоненты Y, в кг.
    /// </summary>
    public double Y { get; set; } = y;
}
