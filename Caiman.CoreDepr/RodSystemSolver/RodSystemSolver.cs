using Caiman.Core.Matrices;
using Caiman.CoreDepr.Matrices;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Caiman.CoreDepr.RodSystemSolver;

public class RodSystemSolver : IRodSystemSolver
{
    private readonly IMatrixBuilder _matrixBuilder;
    private readonly IStiffnessMatrixBuilder _stiffnessMatrixBuilder;

    public RodSystemSolver(IMatrixBuilder matrixBuilder, IStiffnessMatrixBuilder stiffnessMatrixBuilder)
    {
        _matrixBuilder = matrixBuilder;
        _stiffnessMatrixBuilder = stiffnessMatrixBuilder;
    }

    public Matrix<double> FindDisplacementVector(RodSystem system)
    {
        const int dimensions = 2;
        int nodesCount = system.NodeReferences.Count;
        Matrix<double> systemStiffnessMatrix = DenseMatrix.Create(nodesCount * dimensions, nodesCount * dimensions, 0);
        foreach (Element element in system.Elements)
        {
            Matrix<double> elementStiffnessMatrix = CreateGlobalStiffnessMatrix(element);
            int startNodeIndex = element.StartNode.Id;
            int endNodeIndex = element.EndNode.Id;
            AddToSystemStiffnessMatrix(systemStiffnessMatrix, elementStiffnessMatrix, startNodeIndex, endNodeIndex);
        }

        ApplyConstraints(systemStiffnessMatrix, system.NodeReferences);

        // TODO проверить существует ли матрица
        Matrix<double> inverseSystemStiffnessMatrix = systemStiffnessMatrix.Inverse();
        Matrix<double> loadVector = GetLoadVector(system.NodeReferences);

        Matrix<double>? displacementVector = inverseSystemStiffnessMatrix * loadVector;
        return displacementVector;
    }


    private void AddToSystemStiffnessMatrix(Matrix<double> systemStiffnessMatrix,
        Matrix<double> elementStiffnessMatrix, int startNodeIndex, int endNodeIndex)
    {
        foreach ((int, int, double) element in elementStiffnessMatrix.EnumerateIndexed())
        {
            int row = element.Item1 < 2 ? startNodeIndex * 2 + element.Item1 : endNodeIndex * 2 + element.Item1 - 2;
            int col = element.Item2 < 2 ? startNodeIndex * 2 + element.Item2 : endNodeIndex * 2 + element.Item2 - 2;
            systemStiffnessMatrix[row, col] += element.Item3;
        }
    }

    private void ApplyConstraints(Matrix<double> systemStiffnessMatrix, IEnumerable<NodeReference> nodeReferences)
    {
        foreach (NodeReference nodeReference in nodeReferences.Where(n => n.Constraint is not null))
        {
            Constraint constraint = nodeReference.Constraint!;
            if (constraint.U)
            {
                int row = nodeReference.Id * 2;
                int col = nodeReference.Id * 2;
                systemStiffnessMatrix.ClearColumn(col);
                systemStiffnessMatrix.ClearRow(row);
                systemStiffnessMatrix[row, col] = 1;
            }

            if (constraint.V)
            {
                int row = nodeReference.Id * 2 + 1;
                int col = nodeReference.Id * 2 + 1;
                systemStiffnessMatrix.ClearColumn(col);
                systemStiffnessMatrix.ClearRow(row);
                systemStiffnessMatrix[row, col] = 1;
            }
        }
    }

    private Matrix<double> CreateGlobalStiffnessMatrix(Element element)
    {
        Matrix<double> localStiffnessMatrix =
            _stiffnessMatrixBuilder.CreateLocalElementStiffnessMatrix(element, _matrixBuilder.CreateLocalMatrix());

        Matrix<double> globalStiffnessMatrix = _stiffnessMatrixBuilder
            .CreateGlobalElementStiffnessMatrix(localStiffnessMatrix,
                _matrixBuilder.CreateTransformationMatrix(element.Sin, element.Cos));

        return globalStiffnessMatrix;
    }

    private Matrix<double> GetLoadVector(ICollection<NodeReference> nodeReferences)
    {
        // TODO перенести в RodSystem
        const int dimensions = 2;
        var loadVector = DenseMatrix.Create(nodeReferences.Count * dimensions, 1, 0);
        foreach (NodeReference nodeReference in nodeReferences)
        {
            int pos = nodeReference.Id * 2;
            loadVector[pos, 0] = nodeReference.Loads.Sum(l => l.X);
            loadVector[pos + 1, 0] = nodeReference.Loads.Sum(l => l.Y);
        }

        return loadVector;
    }
}
