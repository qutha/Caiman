using Caiman.Core.Optimization.Restrictions;

namespace Caiman.Core.DiscreteSelection;

public class SectionSearcher
{
    public List<List<double>> SelectDiscretelyAllCombinations(IList<double> target, IList<double> discrete,
        IList<OptimizationRestriction> restrictions)
    {
        var availableCombinations = new List<List<double>>();
        foreach (IEnumerable<double> enumerableCombination in GenerateAllCombinations(target, discrete))
        {
            var combination = (List<double>)enumerableCombination;
            var isCorrect = CheckRestrictions(combination, restrictions);
            if (isCorrect)
            {
                availableCombinations.Add(combination);
            }
        }

        return availableCombinations;
    }

    public List<double> SelectOptimalSet(Func<IList<double>, double> targetFunc, List<List<double>> discrete) =>
        discrete.OrderBy(v => targetFunc(v)).First();

    public bool CheckRestrictions(IList<double> target, IList<OptimizationRestriction> restrictions)
    {
        foreach (var restriction in restrictions)
        {
            restriction.UpdateRestriction(target);
        }

        return restrictions.All(r => r.Type != RestrictionType.Violated);
    }

    private IEnumerable<IEnumerable<double>> GenerateAllCombinations(IList<double> targetValues,
        IList<double> discreteValues)
    {
        var availableChoices = new List<List<double>>();

        foreach (var targetValue in targetValues)
        {
            var choices = new List<double>();
            try
            {
                var lowerDiscrete = discreteValues.Last(x => x <= targetValue);
                choices.Add(lowerDiscrete);
            }
            catch (InvalidOperationException)
            {
                // Не добавлять в список, если значение не найдено
            }


            try
            {
                var higherDiscrete = discreteValues.First(x => x >= targetValue);
                choices.Add(higherDiscrete);
            }
            catch (InvalidOperationException)
            {
                // Не добавлять в список, если значение не найдено
            }

            if (choices.Count == 1)
            {
                choices.Add(choices[0]);
            }

            availableChoices.Add(choices);
        }

        List<int> indices = availableChoices.Select(_ => 0).ToList();
        while (!indices.TrueForAll(i => i == 1))
        {
            var combination = new List<double>(indices.Count);
            combination.AddRange(indices.Select((t, i) => availableChoices[i][t]));
            yield return combination;
            Increment(indices);
        }

        yield return indices.Select((t, i) => availableChoices[i][t]).ToList();
    }

    private void Increment(List<int> binaryNumber)
    {
        for (var i = binaryNumber.Count - 1; i >= 0; i--)
        {
            if (binaryNumber[i] == 0)
            {
                binaryNumber[i] = 1;
                break;
            }

            binaryNumber[i] = 0;
        }
    }
}
