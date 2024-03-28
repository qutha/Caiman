namespace Caiman.CoreDepr.RodSystemSolver;

public class ElementProperties(double elasticity, double crossSectionalArea)
{
    /// <summary>
    ///     Модуль упругости, TODO: единицы измерения
    /// </summary>
    public double Elasticity { get; set; } = elasticity;

    /// <summary>
    ///     Площадь поперечного сечения
    /// </summary>
    public double CrossSectionalArea { get; set; } = crossSectionalArea;
}
