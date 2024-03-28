using MathNet.Numerics.LinearAlgebra;

namespace Caiman.Core.Matrices;

public interface IMatrixBuilder
{
    Matrix<double> CreateTransformationMatrix(double sin, double cos);
    Matrix<double> CreateLocalMatrix();
}
