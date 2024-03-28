using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Caiman.Core.Matrices;

public class MatrixBuilder2D : IMatrixBuilder
{
    public Matrix<double> CreateLocalMatrix()
    {
        DenseMatrix matrix = DenseMatrix.OfArray(new double[,]
        {
            { 1, 0, -1, 0 },
            { 0, 0, 0, 0 },
            { -1, 0, 1, 0 },
            { 0, 0, 0, 0 },
        });
        return matrix;
    }

    public Matrix<double> CreateTransformationMatrix(double sin, double cos)
    {
        Matrix<double> matrix = DenseMatrix.OfArray(new[,]
        {
            { cos, sin, 0, 0 },
            { -sin, cos, 0, 0 },
            { 0, 0, cos, sin },
            { 0, 0, -sin, cos },
        });
        return matrix;
    }
}
