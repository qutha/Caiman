﻿using Caiman.Core.Analysis;
using Caiman.Core.Construction;
using Caiman.Core.DiscreteSelection;
using Caiman.Core.Matrices;
using Caiman.Core.Optimization;
using Caiman.Core.Optimization.Restrictions;
using MathNet.Numerics.LinearAlgebra;

{
    // Console.WriteLine("RodSystemSolver");
    //
    // var elementProperties = new ElementProperties(200, 1);
    //
    // var node1 = new Node(0, 0);
    // var node2 = new Node(1, 1);
    // var node3 = new Node(2, 0);
    // var node4 = new Node(3, 3);
    //
    // var element1 = new Element(node1, node2, elementProperties);
    // var element2 = new Element(node2, node3, elementProperties);
    // var element3 = new Element(node3, node1, elementProperties);
    //
    // var force = new ConcentratedLoad(0, -1);
    //
    // var uv = new Constraint(true, true);
    // var v = new Constraint(false, true);
    //
    // var system = new RodSystem();
    // system
    //     .AddElement(element1)
    //     .AddElement(element2)
    //     .AddElement(element3)
    //     .AddLoad(force, node2)
    //     .AddConstraint(uv, node1)
    //     .AddConstraint(v, node3)
    //     .Build();

    #region Hide

    // var node1 = new Node(5, 0);
    // var node2 = new Node(0, 8);
    // var node3 = new Node(3, 6);
    // var node4 = new Node(7, 6);
    //
    // var element1 = new Element(node1, node2, elementProperties);
    // var element2 = new Element(node2, node3, elementProperties);
    // var element3 = new Element(node1, node3, elementProperties);
    // var element4 = new Element(node3, node4, elementProperties);
    // var element5 = new Element(node1, node4, elementProperties);
    //
    // var force1 = new ConcentratedLoad(0, -2.3);

    // var system = new RodSystem();
    // system
    //     .AddElement(element1)
    //     .AddElement(element2)
    //     .AddElement(element3)
    //     .AddElement(element4)
    //     .AddElement(element5)
    //     .AddLoad(force1, node3)
    //     .AddLoad(force1, node4)
    //     .AddConstraint(uv, node1)
    //     .AddConstraint(v, node2)
    //     .Build();

    // var elementProps = new ElementProperties(200, 1.5);
    //
    // var node1 = new Node(0, 0);
    // var node2 = new Node(2, 4);
    // var node3 = new Node(7, 7);
    // var node4 = new Node(12, 4);
    // var node5 = new Node(14, 0);
    //
    // var element1 = new Element(node1, node2, elementProps);
    // var element2 = new Element(node2, node3, elementProps);
    // var element3 = new Element(node3, node4, elementProps);
    // var element4 = new Element(node4, node5, elementProps);
    // var element5 = new Element(node2, node5, elementProps);
    // var element6 = new Element(node1, node4, elementProps);

    // var load1 = new ConcentratedLoad(1, 0);
    // var load2 = new ConcentratedLoad(0, -1);
    //
    // var system = new RodSystem();
    // system
    //     .AddElement(element1)
    //     .AddElement(element2)
    //     .AddElement(element3)
    //     .AddElement(element4)
    //     .AddElement(element5)
    //     .AddElement(element6)
    //     .AddLoad(load1, node2)
    //     .AddLoad(load2, node3)
    //     .AddConstraint(uv, node1)
    //     .AddConstraint(uv, node5)
    //     .Build();

    #endregion Hide

    // var localMatrixBuilder = new MatrixBuilder2D();
    // var stiffnessMatrixBuilder = new StiffnessMatrixBuilder2D();
    //
    //
    // var solver = new RodSystemSolver(localMatrixBuilder, stiffnessMatrixBuilder);
    // Matrix<double> displacementVector = solver.FindDisplacementVector(system);
    //
    // Console.WriteLine("Вектор перемещений");
    // Console.WriteLine(displacementVector);
}
{
    // var wnode1 = new Node(new Vector2(0, 0)) { Id = 0 };
    // var wnode2 = new Node(new Vector2(1, 1)) { Id = 1 };
    // var wnode3 = new Node(new Vector2(2, 0)) { Id = 2 };
    //
    // var welement1 = new Element(wnode1, wnode2, 200, 1);
    // var welement2 = new Element(wnode2, wnode3, 200, 1);
    // var welement3 = new Element(wnode3, wnode1, 200, 1);
    //
    // var wforce = new ConcentratedLoad(new Vector2(0, -1));
    //
    // var wuv = new Constraint(true, true);
    // var wv = new Constraint(false, true);
    //
    // var constructionBuilder = new ConstructionBuilder();
    // ConstructionModel construction = constructionBuilder
    //     .AddNode(wnode1)
    //     .AddNode(wnode2)
    //     .AddNode(wnode3)
    //     .AddElement(welement1)
    //     .AddElement(welement2)
    //     .AddElement(welement3)
    //     .AddLoad(wnode2, wforce)
    //     .AddConstraint(wnode1, wuv)
    //     .AddConstraint(wnode3, wv)
    //     .Build();
    // var matrixBuilder = new MatrixBuilder2D();
    // var analyzer = new ConstructionAnalyzer(matrixBuilder);
    //
    // MathNet.Numerics.LinearAlgebra.Vector<double> vector = analyzer.FindDisplacementVector(construction);
    // Console.WriteLine("\n\n\n");
    // Console.WriteLine(vector);
    //
    // Func<double[], double> func = analyzer.GenerateDisplacementOnAreasFunction(construction, 1, Axis.Y);
    // double[] areas = construction.Elements.Select(e => e.Area).ToArray();
    //
    // var derivativeFinder = new MultiDerivative();
    // var gradientFinder = new Gradient(derivativeFinder);
    //
    // MathNet.Numerics.LinearAlgebra.Vector<double> gradient = gradientFinder.GetGradient(func, areas);
    // Console.WriteLine(gradient);

    // Console.WriteLine(func(areas));
    // MathNet.Numerics.LinearAlgebra.Vector<double> vector2 = analyzer.FindDisplacementVector(construction);
    // Console.WriteLine(vector2);
}

// {
//     Func<double[], double> func = args => Math.Pow(args[0], 2) + 2 * args[1];
//     var derivativeFinder = new MultiDerivative();
//     var gradientFinder = new Gradient(derivativeFinder);
//     MathNet.Numerics.LinearAlgebra.Vector<double> gradient = gradientFinder.GetGradient(func, [2.0, 2.0]);
//     MathNet.Numerics.LinearAlgebra.Vector<double> antiGradient =
//         gradientFinder.GetAntiGradient(func, [2.0, 2.0]);
//
//     Console.WriteLine(gradient);
//     Console.WriteLine(antiGradient);
// }
//
// {
//     Func<double[], double> func = args => Math.Pow(args[0], 2) + 2 * args[1];
//     var derivativeFinder = new MultiDerivative();
//     var gradientFinder = new Gradient(derivativeFinder);
//     MathNet.Numerics.LinearAlgebra.Vector<double> gradient = gradientFinder.GetGradient(func, [2.0, 2.0]);
//     MathNet.Numerics.LinearAlgebra.Vector<double> antiGradient =
//         gradientFinder.GetAntiGradient(func, [2.0, 2.0]);
//
//     Console.WriteLine(gradient);
//     Console.WriteLine(antiGradient);
// }

#region Мой вариант

{
    // Мой вариант


    var node1 = new Node(0, 700);
    var node2 = new Node(1200, 900);
    var node3 = new Node(1400, 0);
    var node4 = new Node(0, 0);

    const double elasticity = 2_000_000;
    const int area = 14;

    var element1 = new Element(node1, node2, elasticity, area);
    var element2 = new Element(node2, node3, elasticity, area);
    var element3 = new Element(node3, node4, elasticity, area);
    var element4 = new Element(node4, node1, elasticity, area);
    var element5 = new Element(node1, node3, elasticity, area);
    var element6 = new Element(node4, node2, elasticity, area);

    var load = new ConcentratedLoad(10_000, 0);

    var uv = new Constraint(true, true);
    var v = new Constraint(false, true);

    var constructionBuilder = new ConstructionBuilderFactory().CreateBuilder();

    var construction = constructionBuilder
        .AddNode(node1)
        .AddNode(node2)
        .AddNode(node3)
        .AddNode(node4)
        .AddElement(element1)
        .AddElement(element2)
        .AddElement(element3)
        .AddElement(element4)
        .AddElement(element5)
        .AddElement(element6)
        .AddLoad(node2, load)
        .AddConstraint(node3, v)
        .AddConstraint(node4, uv)
        .Build();

    var matrixBuilder = new MatrixBuilder2D();
    var analyzer = new ConstructionAnalyzer(matrixBuilder);

    Vector<double> displacementVector = analyzer.FindDisplacementVector(construction);
    Console.WriteLine("Вектор перемещений (см)");
    Console.WriteLine(displacementVector);

    Func<IList<double>, double> nodeDisplacementFunc =
        analyzer.GenerateNodeDisplacementOnAreasFunction(construction, node2, Axis.X);

    var areas = construction.Elements.Select(el => el.Area).ToArray();
    var internalForces1 = analyzer.FindInternalForces(construction, element1);
    var internalForces2 = analyzer.FindInternalForces(construction, element2);
    var internalForces3 = analyzer.FindInternalForces(construction, element3);
    var internalForces4 = analyzer.FindInternalForces(construction, element4);
    var internalForces5 = analyzer.FindInternalForces(construction, element5);
    var internalForces6 = analyzer.FindInternalForces(construction, element6);
    Console.WriteLine("Внутренние усилия");
    Console.WriteLine(internalForces1);
    Console.WriteLine(internalForces2);
    Console.WriteLine(internalForces3);
    Console.WriteLine(internalForces4);
    Console.WriteLine(internalForces5);
    Console.WriteLine(internalForces6);

    var stresses1 = analyzer.FindStresses(construction, element1);
    var stresses2 = analyzer.FindStresses(construction, element2);
    var stresses3 = analyzer.FindStresses(construction, element3);
    var stresses4 = analyzer.FindStresses(construction, element4);
    var stresses5 = analyzer.FindStresses(construction, element5);
    var stresses6 = analyzer.FindStresses(construction, element6);

    Console.WriteLine("Напряжения");
    Console.WriteLine(stresses1);
    Console.WriteLine(stresses2);
    Console.WriteLine(stresses3);
    Console.WriteLine(stresses4);
    Console.WriteLine(stresses5);
    Console.WriteLine(stresses6);

    var derivativeFinder = new DerivativeFinder();
    var gradientFinder = new GradientFinder(derivativeFinder);

    Vector<double> gradient = gradientFinder.FindGradient(nodeDisplacementFunc, areas);

    Console.WriteLine("\nГрадиент перемещения узла 2 вдоль оси X");
    Console.WriteLine(gradient);

    var restrictionBuilder = new RestrictionBuilder();
    List<OptimizationRestriction> restrictions = restrictionBuilder
        .Add(new NodeDisplacementRestriction(nodeDisplacementFunc,
                Math.Abs(nodeDisplacementFunc(analyzer.GetAreasVector(construction)))
            )
        )
        .Add(AreaRestriction.CreateRestrictionForAll(construction, 10))
        .Build();

    var optimizer = new ConstructionOptimizer(analyzer, gradientFinder);
    Func<IList<double>, double> targetFunc = analyzer.GenerateMaterialConsumptionFunction(construction);
    Console.WriteLine(
        $"Материалоемкость до оптимизации {analyzer.GenerateMaterialConsumptionFunction(construction)(areas)}");
    IList<double> optimizedAreas = optimizer.Optimize(construction, restrictions, OptimizationOptions.Default);

    Console.WriteLine(optimizedAreas);

    Console.WriteLine(
        $"Материалоемкость после оптимизации {targetFunc(optimizedAreas)}"
    );

    Console.WriteLine($"Эффективность до подбора: {optimizer.GetEfficiency(targetFunc, areas, optimizedAreas)}");

    var sectionSearcher = new SectionSearcher();
    List<List<double>> discreteAreasCombinations =
        sectionSearcher.SelectDiscretelyAllCombinations(optimizedAreas, Section.GetTubeSections(), restrictions);
    Console.WriteLine("Оптимальные значения после подбора");
    List<double> optimal = sectionSearcher.SelectOptimalSet(analyzer.GenerateMaterialConsumptionFunction(construction),
        discreteAreasCombinations);
    foreach (var op in optimal)
    {
        Console.WriteLine(op);
    }

    Console.WriteLine($"Эффективность после подбора: {optimizer.GetEfficiency(targetFunc, areas, optimal)}");
}


// TODO FluentConstructionAnalyzer

#endregion Мой вариант

#region Serialization Test

//
// {
//     var node0 = new Node(0, 7);
//     var node1 = new Node(12, 9);
//     var node2 = new Node(14, 0);
//     var node3 = new Node(0, 0);
//
//     var element0 = new Element(node0, node1, 21_000_000, 40_000);
//     var element1 = new Element(node1, node2, 21_000_000, 40_000);
//     var element2 = new Element(node2, node3, 21_000_000, 40_000);
//     var element3 = new Element(node3, node0, 21_000_000, 40_000);
//     var element4 = new Element(node0, node2, 21_000_000, 40_000);
//
//     var load = new ConcentratedLoad(1, 0);
//
//     var uv = new Constraint(true, true);
//     var v = new Constraint(false, true);
//
//
//     var constructionBuilder = new ConstructionBuilder();
//
//     ConstructionModel construction = constructionBuilder
//         .AddNode(node0)
//         .AddNode(node1)
//         .AddNode(node2)
//         .AddNode(node3)
//         .AddElement(element0)
//         .AddElement(element1)
//         .AddElement(element2)
//         .AddElement(element3)
//         .AddElement(element4)
//         .AddLoad(node1, load)
//         .AddConstraint(node2, v)
//         .AddConstraint(node3, uv)
//         .Build();
//
//
// ConstructionDto constructionDto = ConstructionDto.FromModel(construction);
// var serializer = new ConstructionSerializer();
//
// string json = serializer.Serialize(constructionDto);
// Console.WriteLine(json);
//
// ConstructionDto deserializedConstruction = serializer.Deserialize(json);
//     ConstructionModel newConstruction = deserializedConstruction.ToModel(new ConstructionBuilder());
//     // var newSerializer = new NewConstructionSerializer();
//     // string newJson = newSerializer.Serialize(construction);
//     // Console.WriteLine(newJson);
//     var matrixBuilder = new MatrixBuilder2D();
//     var analyzer = new ConstructionAnalyzer(matrixBuilder);
//
//     Vector<double> displacementVector = analyzer.FindDisplacementVector(newConstruction);
//
//     Func<IList<double>, double> func = analyzer.GenerateDisplacementOnAreasFunction(construction, 1, Axis.X);
//     double[] areas = construction.Elements.Select(el => el.Area).ToArray();
//
//     Console.WriteLine("Вектор перемещений");
//     Console.WriteLine(displacementVector);
// }

#endregion Serialization Test

#region MultiGradientDescent

{
    // Func<double[], double> multifunc = x => 6 * Math.Pow(x[0], 2) + 3 * Math.Pow(x[1], 2) - 10 * x[0] - x[1] - 10;
    // var descent = new Descent(multifunc, [0, 0], 0.5, 0.00001);
    // double min = descent.Min();
    // Console.WriteLine(min);
}

#endregion MultiGradientDescent
