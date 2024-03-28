namespace Caiman.Core.Optimization;

public class OptimizationOptions
{
    public static OptimizationOptions Default => new();

    public bool UsePrecision { get; set; } = false;
    public bool UseMaxIterations { get; set; } = true;
    public bool UseEfficiencyPerStep { get; set; } = true;

    /// <summary>
    ///     <para>Оптимизировать систему, пока все ограничения не будут выполнены</para>
    ///     <para>Учитывает остальные опции, но может потребовать больше итераций</para>
    /// </summary>
    // public bool UseCorrectRestrictions { get; set; } = false;

    public double Precision { get; set; } = Constants.Epsilon;

    public double MaxIterations { get; set; } = 100;
    public double EfficiencyPerStep { get; set; } = 0.0001;

    public bool CheckPrecision(double precision) => Math.Abs(precision) < Math.Abs(Precision);
    public bool CheckIterations(int iterations) => iterations >= MaxIterations;
    public bool CheckEfficiencyPerStep(double efficiencyPerStep) => efficiencyPerStep <= EfficiencyPerStep;
}
