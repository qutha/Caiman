using Caiman.Core.Construction;
using Caiman.Core.Construction.Exceptions;
using Caiman.Core.Matrices;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Caiman.Core.Analysis;

public class ConstructionAnalyzer(IMatrixBuilder matrixBuilder)
{
    #region Fields

    private const int Dimensions = 2;

    #endregion

    #region Function Generators

    /// <summary>
    ///     Метод для генерации функции перемещения узла вдоль оси от площадей стержней
    /// </summary>
    /// <param name="construction">Стержневая система</param>
    /// <param name="nodeEntity">Индекс узла</param>
    /// <param name="axis">Ось, вдоль которой перемещается узел</param>
    /// <returns>Функция перемещения узла вдоль оси от площадей стержней</returns>
    /// <exception cref="NodeNotFoundException">Узел не находится внутри системы</exception>
    public Func<IList<double>, double> GenerateNodeDisplacementOnAreasFunction(ConstructionEntity construction,
        NodeEntity nodeEntity,
        Axis axis)
    {
        NodeIndex nodeIndex = construction.IndexOf(nodeEntity);
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
                Axis.X => vector[nodeIndex * Dimensions],
                Axis.Y => vector[nodeIndex * Dimensions + 1],
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

    /// <summary>
    ///     Метод для генерации функции расхода материала от площадей стержней
    /// </summary>
    /// <param name="construction">Стержневая система</param>
    /// <returns>Функцию расхода материала от площадей стержней</returns>
    public Func<IList<double>, double> GenerateMaterialConsumptionFunction(ConstructionEntity construction)
    {
        Func<IList<double>, double> function = areas =>
        {
            return areas.Select((t, i) => construction.Elements[i].Length * t).Sum();
        };

        return function;
    }

    #endregion

    #region Analysis

    /// <summary>
    ///     Метод для нахождения вектора перемещения
    /// </summary>
    /// <param name="construction">Стержневая система</param>
    /// <returns>Вектор перемещения в см</returns>
    public Vector<double> FindDisplacementVector(ConstructionEntity construction)
    {
        Matrix<double> constructionStiffnessMatrix =
            CreateGlobalConstructionStiffnessMatrixWithConstraints(construction);
        Vector<double> loadVector = GetLoadVector(construction);
        Matrix<double> inverseConstructionStiffnessMatrix = constructionStiffnessMatrix.Inverse();
        Vector<double> displacementVector = inverseConstructionStiffnessMatrix * loadVector;
        return displacementVector;
    }

    /// <summary>
    ///     <para>
    ///         <b>Метод для нахождения вектора внутренних усилий для стержня</b>
    ///     </para>
    ///     <para>Если вектор перемещения не задан, то произойдет расчет всей системы</para>
    ///     <para>Рекомендуется заранее вычислить вектор перемещения</para>
    /// </summary>
    /// <param name="construction">Стержневая система</param>
    /// <param name="elementEntity">Стержень, в котором требуется найти внутренние усилия</param>
    /// <param name="displacementVector">Вектор перемещения, если не задан, то произойдет расчет всей системы</param>
    /// <returns>Вектор внутренних усилий в кг</returns>
    /// <exception cref="ElementNotFoundException">Стержень не находится внутри системы</exception>
    public Vector<double> FindInternalForces(ConstructionEntity construction, ElementEntity elementEntity,
        Vector<double>? displacementVector = null)
    {
        Vector<double> displacement = displacementVector ?? FindDisplacementVector(construction);
        (NodeIndex startIndex, NodeIndex endIndex) = construction.GetNodesIndicesFromElement(elementEntity);
        Vector<double>? elementDisplacement = Vector<double>.Build.DenseOfArray(
            [
                displacement[startIndex * Dimensions],
                displacement[startIndex * Dimensions + 1],
                displacement[endIndex * Dimensions],
                displacement[endIndex * Dimensions + 1],
            ]
        );
        Matrix<double> elementLocalStiffnessMatrix = CreateLocalElementStiffnessMatrix(elementEntity);
        Matrix<double> transformationMatrix =
            matrixBuilder.CreateTransformationMatrix(elementEntity.Sin, elementEntity.Cos);

        Vector<double> internalForces = elementLocalStiffnessMatrix * transformationMatrix * elementDisplacement;
        return internalForces;
    }

    /// <summary>
    ///     <para>
    ///         <b>Метод для нахождения напряжений в стержне</b>
    ///     </para>
    ///     <para>Если вектор внутренних усилий не задан, то произойдет расчет всей системы</para>
    ///     <para>Рекомендуется заранее вычислить вектор перемещения</para>
    /// </summary>
    /// <param name="construction">Стержневая система</param>
    /// <param name="elementEntity">Стержень, в котором требуется найти напряжения</param>
    /// <param name="internalForces">Вектор внутренних усилий, если не задан, то произойдет расчет всей системы</param>
    /// <returns>Вектор напряжений в кг/см2</returns>
    public Vector<double> FindStresses(ConstructionEntity construction, ElementEntity elementEntity,
        Vector<double>? internalForces = null)
    {
        Vector<double> internalForcesVector = internalForces ?? FindInternalForces(construction, elementEntity);
        Vector<double> stresses = internalForcesVector / elementEntity.Area;
        return stresses;
    }

    public Vector<double> GetLoadVector(ConstructionEntity construction)
    {
        Vector<double> loadVector = Vector<double>.Build.Sparse(construction.Nodes.Count * Dimensions);
        foreach (NodeEntity node in construction.Nodes)
        {
            int nodeIndex = construction.IndexOf(node);
            int pos = nodeIndex * 2;
            loadVector[pos] = node.LoadList.Sum(l => l.X);
            loadVector[pos + 1] = node.LoadList.Sum(l => l.Y);
        }

        return loadVector;
    }

    public Vector<double> GetAreasVector(ConstructionEntity construction)
    {
        Vector<double> areasVector = Vector<double>.Build.DenseOfEnumerable(
            construction.Elements.Select(el => el.Area));
        return areasVector;
    }

    #endregion

    #region Matrices

    public Matrix<double> CreateGlobalElementStiffnessMatrix(ConstructionEntity construction,
        ElementEntity elementEntity)
    {
        Matrix<double> localStiffnessMatrix = CreateLocalElementStiffnessMatrix(elementEntity);
        Matrix<double> transformationMatrix =
            matrixBuilder.CreateTransformationMatrix(elementEntity.Sin, elementEntity.Cos);
        Matrix<double> globalStiffnessMatrix =
            transformationMatrix.Transpose() * localStiffnessMatrix * transformationMatrix;
        return globalStiffnessMatrix;
    }

    public Matrix<double> CreateLocalElementStiffnessMatrix(ElementEntity elementEntity)
    {
        Matrix<double> localStiffnessMatrix = elementEntity.Stiffness * matrixBuilder.CreateLocalMatrix();
        return localStiffnessMatrix;
    }

    public Matrix<double> CreateGlobalConstructionStiffnessMatrix(ConstructionEntity construction)
    {
        int nodesCount = construction.Nodes.Count;
        var constructionStiffnessMatrix = SparseMatrix.Create(nodesCount * Dimensions, nodesCount * Dimensions, 0);
        foreach (ElementEntity element in construction.Elements)
        {
            Matrix<double> elementStiffnessMatrix = CreateGlobalElementStiffnessMatrix(construction, element);
            (NodeIndex startNodeIndex, NodeIndex endNodeIndex) = construction.GetNodesIndicesFromElement(element);
            AddToConstructionStiffnessMatrix(constructionStiffnessMatrix, elementStiffnessMatrix, startNodeIndex,
                endNodeIndex);
        }

        return constructionStiffnessMatrix;
    }

    public Matrix<double> CreateGlobalConstructionStiffnessMatrixWithConstraints(ConstructionEntity construction)
    {
        Matrix<double> globalStiffnessMatrix = CreateGlobalConstructionStiffnessMatrix(construction);
        ApplyConstraints(construction, globalStiffnessMatrix);
        return globalStiffnessMatrix;
    }

    #endregion Matrices

    #region Private Methods

    private void AddToConstructionStiffnessMatrix(Matrix<double> constructionStiffnessMatrix,
        Matrix<double> elementStiffnessMatrix, NodeIndex startNodeIndex, NodeIndex endNodeIndex)
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

    private void ApplyConstraints(ConstructionEntity construction, Matrix<double> constructionStiffnessMatrix)
    {
        foreach (NodeEntity node in construction.Nodes.Where(n => n.Constraint is not null))
        {
            Constraint constraint = node.Constraint!;
            int nodeIndex = construction.IndexOf(node);
            if (constraint.X)
            {
                int row = nodeIndex * Dimensions;
                int col = nodeIndex * Dimensions;
                constructionStiffnessMatrix.ClearColumn(col);
                constructionStiffnessMatrix.ClearRow(row);
                constructionStiffnessMatrix[row, col] = 1;
            }

            if (constraint.Y)
            {
                int row = nodeIndex * Dimensions + 1;
                int col = nodeIndex * Dimensions + 1;
                constructionStiffnessMatrix.ClearColumn(col);
                constructionStiffnessMatrix.ClearRow(row);
                constructionStiffnessMatrix[row, col] = 1;
            }
        }
    }

    #endregion Private Methods
}
