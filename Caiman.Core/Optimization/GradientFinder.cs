using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Caiman.Core.Optimization;

public class GradientFinder(DerivativeFinder derivativeFinderFinder)
{
    public Vector<double> FindGradient(Func<IList<double>, double> func, IList<double> args)
    {
        Vector<double> gradient = DenseVector.Create(args.Count, 0);
        for (var i = 0; i < args.Count; i++)
        {
            gradient[i] = derivativeFinderFinder.FindDerivative(func, args, (uint)i);
        }

        return gradient;
    }

    public Vector<double> FindAntiGradient(Func<IList<double>, double> func, IList<double> args)
    {
        Vector<double> gradient = FindGradient(func, args);

        return -gradient;
    }
}
