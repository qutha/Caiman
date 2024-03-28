using MathNet.Numerics.LinearAlgebra;

namespace Caiman.CoreDepr.RodSystemSolver;

public interface IRodSystemSolver
{
    Matrix<double> FindDisplacementVector(RodSystem system);
}
