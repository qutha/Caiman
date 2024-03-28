using Caiman.CoreDepr.RodSystemSolver;
using MathNet.Numerics.LinearAlgebra;

namespace Caiman.CoreDepr.Matrices;

public interface IStiffnessMatrixBuilder
{
    Matrix<double> CreateLocalElementStiffnessMatrix(Element element, Matrix<double> localMatrix);

    Matrix<double> CreateGlobalElementStiffnessMatrix(Matrix<double> localStiffnessMatrix,
        Matrix<double> transformationMatrix);

    Matrix<double> CreateSystemStiffnessMatrix(RodSystem system);
}
