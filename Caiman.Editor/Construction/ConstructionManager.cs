using System.Text;
using Caiman.Core.Analysis;
using Caiman.Core.Construction;
using Caiman.Core.DiscreteSelection;
using Caiman.Core.Optimization;
using Caiman.Core.Optimization.Restrictions;
using MathNet.Numerics.LinearAlgebra;

namespace Caiman.Editor.Construction;

public class ConstructionManager(
    ConstructionAnalyzer analyzer,
    ConstructionOptimizer optimizer,
    SectionSearcher sectionSearcher,
    EditorConstruction editorConstruction)
{
    public Core.Construction.Construction ToConstruction(EditorConstruction edConstruction)
    {
        var construction = new Core.Construction.Construction();
        IEnumerable<Node> nodes = edConstruction.Nodes.Select(n => new Node(n.X, -n.Y)
        {
            Constraint = n.Constraint,
            Loads = n.Loads,
        });
        construction.Nodes.AddRange(nodes);

        IEnumerable<Element> elements = edConstruction.Elements.Select(el =>
            new Element(construction.Nodes[el.StartNode.Id], construction.Nodes[el.EndNode.Id], el.Elasticity, el.Area)
        );
        construction.Elements.AddRange(elements);

        return construction;
    }

    public string FindNodalDisplacement()
    {
        var construction = ToConstruction(editorConstruction);
        Vector<double> displacements = analyzer.FindDisplacementVector(construction);

        var result = new StringBuilder();
        result.AppendLine("Nodal Displacements:");
        for (var i = 0; i < displacements.Count - 1; i += 2)
        {
            var nodeId = i / 2;
            result.AppendLine($"\tNode {nodeId}:");
            result.AppendLine($"\t\tX {displacements[i]}");
            result.AppendLine($"\t\tY {displacements[i + 1]}");
        }

        result.AppendLine();
        return result.ToString();
    }

    public string Optimize()
    {
        var construction = ToConstruction(editorConstruction);

        Func<IList<double>, double> nodeDisplacementFunc =
            analyzer.GenerateNodeDisplacementOnAreasFunction(construction, construction.Nodes[1], Axis.X);
        var restrictionBuilder = new RestrictionBuilder();
        List<OptimizationRestriction> restrictions = restrictionBuilder
            .Add(new NodeDisplacementRestriction(nodeDisplacementFunc,
                    Math.Abs(nodeDisplacementFunc(analyzer.GetAreasVector(construction)))
                )
            )
            .Add(AreaRestriction.CreateRestrictionForAll(construction, 10))
            .Build();

        IList<double> optimizedAreas = optimizer.Optimize(construction, restrictions, OptimizationOptions.Default);
        var result = new StringBuilder();
        result.AppendLine("Optimized Areas:");
        for (var i = 0; i < optimizedAreas.Count; i++)
        {
            result.AppendLine($"\tElement {i}:");
            result.AppendLine($"\t\tArea: {optimizedAreas[i]}");
        }

        var areas = construction.Elements.Select(el => el.Area).ToArray();
        Func<IList<double>, double> targetFunc = analyzer.GenerateMaterialConsumptionFunction(construction);

        result.AppendLine($"Efficiency: {optimizer.GetEfficiency(targetFunc, areas, optimizedAreas)}");
        result.AppendLine();
        return result.ToString();
    }

    public string SelectDiscreteAreas()
    {
        var construction = ToConstruction(editorConstruction);

        Func<IList<double>, double> nodeDisplacementFunc =
            analyzer.GenerateNodeDisplacementOnAreasFunction(construction, construction.Nodes[1], Axis.X);
        var restrictionBuilder = new RestrictionBuilder();
        List<OptimizationRestriction> restrictions = restrictionBuilder
            .Add(new NodeDisplacementRestriction(nodeDisplacementFunc,
                    Math.Abs(nodeDisplacementFunc(analyzer.GetAreasVector(construction)))
                )
            )
            .Add(AreaRestriction.CreateRestrictionForAll(construction, 10))
            .Build();

        IList<double> optimizedAreas = optimizer.Optimize(construction, restrictions, OptimizationOptions.Default);
        List<List<double>> discreteAreasCombinations =
            sectionSearcher.SelectDiscretelyAllCombinations(optimizedAreas, Section.GetTubeSections(), restrictions);
        List<double> optimalAreas = sectionSearcher.SelectOptimalSet(
            analyzer.GenerateMaterialConsumptionFunction(construction),
            discreteAreasCombinations);

        var result = new StringBuilder();
        result.AppendLine("Optimal Areas:");
        for (var i = 0; i < optimizedAreas.Count; i++)
        {
            result.AppendLine($"\tElement {i}:");
            result.AppendLine($"\t\tArea: {optimalAreas[i]}");
        }

        var areas = construction.Elements.Select(el => el.Area).ToArray();
        Func<IList<double>, double> targetFunc = analyzer.GenerateMaterialConsumptionFunction(construction);

        result.AppendLine($"Efficiency: {optimizer.GetEfficiency(targetFunc, areas, optimalAreas)}");
        result.AppendLine();
        return result.ToString();
    }
}
