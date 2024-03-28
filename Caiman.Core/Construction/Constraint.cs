namespace Caiman.Core.Construction;

public class Constraint(bool x, bool y)
{
    public bool X { get; set; } = x;
    public bool Y { get; set; } = y;
}
