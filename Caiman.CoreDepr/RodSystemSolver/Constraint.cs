namespace Caiman.CoreDepr.RodSystemSolver;

public class Constraint(bool u, bool v)
{
    public bool U { get; set; } = u;
    public bool V { get; set; } = v;
}
