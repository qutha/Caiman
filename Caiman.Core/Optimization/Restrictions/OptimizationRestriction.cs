namespace Caiman.Core.Optimization.Restrictions;

public abstract class OptimizationRestriction
{
    public RestrictionType Type { get; private set; }
    public double Value { get; private set; }
    protected abstract double GetValue(IList<double> areas);

    public Func<IList<double>, double> GetRestrictionFunc() => GetValue;

    public void UpdateRestriction(IList<double> areas)
    {
        Value = GetValue(areas);
        var type = Value switch
        {
            < 0 => RestrictionType.Passive,
            _ when Math.Abs(Value) < Constants.Epsilon => RestrictionType.Active,
            > 0 => RestrictionType.Violated,
            _ => throw new Exception("Invalid operation"),
        };
        Type = type;
    }
}
