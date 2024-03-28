using Caiman.Core.Depr.Construction.Exceptions;
using Caiman.Core.Matrices;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Caiman.Core.Depr.Construction;

public class ConstructionAnalyzer(IMatrixBuilder matrixBuilder)
{
    private const int Dimensions = 2;

    public Vector<double> FindDisplacementVector(IConstructionModel construction)
    {
        Matrix<double> constructionStiffnessMatrix =
            CreateGlobalConstructionStiffnessMatrixWithConstraints(construction);
        Vector<double> loadVector = GetLoadVector(construction);
        Matrix<double>? inverseConstructionStiffnessMatrix = constructionStiffnessMatrix.Inverse();
        Vector<double>? displacementVector = inverseConstructionStiffnessMatrix * loadVector;
        return displacementVector;
    }

    public Matrix<double> CreateGlobalElementStiffnessMatrix(IConstructionModel construction, uint elementId)
    {
        Element? element = construction.Elements.FirstOrDefault(el => el.Id == elementId);
        if (element is null)
        {
            throw new ElementNotFoundException();
        }

        Matrix<double> localStiffnessMatrix = CreateLocalElementStiffnessMatrix(construction, elementId);
        Matrix<double> transformationMatrix = matrixBuilder.CreateTransformationMatrix(element.Sin, element.Cos);
        Matrix<double>? globalStiffnessMatrix =
            transformationMatrix.Transpose() * localStiffnessMatrix * transformationMatrix;
        return globalStiffnessMatrix;
    }

    public Matrix<double> CreateLocalElementStiffnessMatrix(IConstructionModel construction, uint elementId)
    {
        Element? element = construction.Elements.FirstOrDefault(el => el.Id == elementId);
        if (element is null)
        {
            throw new ElementNotFoundException();
        }

        Matrix<double> localStiffnessMatrix = element.Stiffness * matrixBuilder.CreateLocalMatrix();
        return localStiffnessMatrix;
    }

    public Matrix<double> CreateGlobalConstructionStiffnessMatrix(IConstructionModel construction)
    {
        int nodesCount = construction.Nodes.Count;
        var constructionStiffnessMatrix = SparseMatrix.Create(nodesCount * Dimensions, nodesCount * Dimensions, 0);
        foreach (Element element in construction.Elements)
        {
            Matrix<double> elementStiffnessMatrix = CreateGlobalElementStiffnessMatrix(construction, (uint)element.Id);
            int startNodeIndex = element.StartNode.Id;
            int endNodeIndex = element.EndNode.Id;
            AddToConstructionStiffnessMatrix(constructionStiffnessMatrix, elementStiffnessMatrix, startNodeIndex,
                endNodeIndex);
        }

        return constructionStiffnessMatrix;
    }

    private void AddToConstructionStiffnessMatrix(Matrix<double> constructionStiffnessMatrix,
        Matrix<double> elementStiffnessMatrix, int startNodeIndex, int endNodeIndex)
    {
        foreach ((int row, int col, double value) in elementStiffnessMatrix.EnumerateIndexed())
        {
            int nodeRow = row < Dimensions
                ? startNodeIndex * Dimensions + row
                : endNodeIndex * Dimensions + row - Dimensions;
            int nodeCol = col < Dimensions
                ? startNodeIndex * Dimensions + col
                : endNodeIndex * Dimensions + col - Dimensions;
            constructionStiffnessMatrix[nodeRow, nodeCol] += value;
        }
    }

    public Matrix<double> CreateGlobalConstructionStiffnessMatrixWithConstraints(IConstructionModel construction)
    {
        Matrix<double> globalStiffnessMatrix = CreateGlobalConstructionStiffnessMatrix(construction);
        ApplyConstraints(construction, globalStiffnessMatrix);
        return globalStiffnessMatrix;
    }

    private void ApplyConstraints(IConstructionModel construction, Matrix<double> constructionStiffnessMatrix)
    {
        foreach (Node node in construction.Nodes.Where(n => n.Constraint is not null))
        {
            Constraint constraint = node.Constraint!;
            if (constraint.X)
            {
                int row = node.Id * Dimensions;
                int col = node.Id * Dimensions;
                constructionStiffnessMatrix.ClearColumn(col);
                constructionStiffnessMatrix.ClearRow(row);
                constructionStiffnessMatrix[row, col] = 1;
            }

            if (constraint.Y)
            {
                int row = node.Id * Dimensions + 1;
                int col = node.Id * Dimensions + 1;
                constructionStiffnessMatrix.ClearColumn(col);
                constructionStiffnessMatrix.ClearRow(row);
                constructionStiffnessMatrix[row, col] = 1;
            }
        }
    }

    public Vector<double> GetLoadVector(IConstructionModel construction)
    {
        Vector<double> loadVector = Vector<double>.Build.Sparse(construction.Nodes.Count * Dimensions);
        foreach (Node node in construction.Nodes)
        {
            int pos = node.Id * 2;
            loadVector[pos] = node.Loads.Sum(l => l.Value.X);
            loadVector[pos + 1] = node.Loads.Sum(l => l.Value.Y);
        }

        return loadVector;
    }

    public Func<IList<double>, double> GenerateDisplacementOnAreasFunction(IConstructionModel construction, uint nodeId,
        Axis axis)
    {
        if (nodeId >= construction.Nodes.Count)
        {
            throw new NodeNotFoundException();
        }

        Func<IList<double>, double> function = areas =>
        {
            double[] initialAreas = construction.Elements.Select(el => el.Area).ToArray();
            for (var i = 0; i < areas.Count; i++)
            {
                construction.Elements[i].Area = areas[i];
            }

            Vector<double> vector = FindDisplacementVector(construction);
            double displacement = axis switch
            {
                Axis.X => vector[(int)(nodeId * Dimensions)],
                Axis.Y => vector[(int)(nodeId * Dimensions + 1)],
                _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null),
            };

            for (var i = 0; i < areas.Count; i++)
            {
                construction.Elements[i].Area = initialAreas[i];
            }

            return displacement;
        };

        return function;
    }
}
