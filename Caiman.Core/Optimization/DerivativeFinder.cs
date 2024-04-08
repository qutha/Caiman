namespace Caiman.Core.Optimization;

public class DerivativeFinder
{
    /// <summary>
    ///     Нахождение первой производной от функции с N параметрами
    /// </summary>
    /// <param name="func">Функция от N переменных</param>
    /// <param name="args">Массив аргументов функции</param>
    /// <param name="index">Индекс аргумента для нахождения производной</param>
    /// <param name="eps">Точность</param>
    /// <returns>Значение производной index с аргументами args</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public double FindDerivative(
        Func<IList<double>, double> func,
        IList<double> args,
        uint index,
        double eps = Constants.Epsilon)
    {
        if (index >= args.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be less than array length");
        }

        var initialValue = args[(int)index];
        args[(int)index] = initialValue + eps;
        var valueRight = func(args);

        args[(int)index] = initialValue - eps;
        var valueLeft = func(args);

        args[(int)index] = initialValue;

        var delta = valueRight - valueLeft;
        var derivative = delta / (2 * eps);
        return derivative;
    }
}