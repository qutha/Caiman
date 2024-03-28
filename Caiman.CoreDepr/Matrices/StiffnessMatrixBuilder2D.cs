using Caiman.CoreDepr.RodSystemSolver;
using MathNet.Numerics.LinearAlgebra;

namespace Caiman.CoreDepr.Matrices;

public class StiffnessMatrixBuilder2D : IStiffnessMatrixBuilder
{
    public Matrix<double> CreateLocalElementStiffnessMatrix(Element element, Matrix<double> localMatrix)
    {
        Matrix<double> stiffnessMatrix = element.Stiffness * localMatrix;
        return stiffnessMatrix;
    }

    public Matrix<double> CreateGlobalElementStiffnessMatrix(Matrix<double> localStiffnessMatrix,
        Matrix<double> transformationMatrix)
    {
        Matrix<double> globalStiffnessMatrix =
            transformationMatrix.Transpose() * localStiffnessMatrix * transformationMatrix;
        return globalStiffnessMatrix;
    }

    public Matrix<double> CreateSystemStiffnessMatrix(RodSystem system) => throw new NotImplementedException();
}
