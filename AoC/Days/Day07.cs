namespace AoC
{
    public sealed class Day07 : BaseDay
    {
        private readonly List<EquationInput> _equationInputs = new();
        private Operator[]? _acceptedOperators;

        public Day07()
        {
            string[] lines = File.ReadAllLines(InputFilePath);
            foreach (string line in lines)
            {
                string[] segments = line.Split(':');
                long result = long.Parse(segments[0]);
                long[] values = segments[1].Trim().Split(' ').Select(long.Parse).ToArray();
                _equationInputs.Add(new EquationInput(values, result));
            }
        }

        public override ValueTask<string> Solve_1()
        {
            _acceptedOperators = new[]{ Operator.Add, Operator.Multiply };
            long result = _equationInputs.Where(IsValidInput).Sum(input => input.ExpectedResult);

            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            _acceptedOperators = new[]{ Operator.Add, Operator.Multiply, Operator.Concatenate };
            long result = _equationInputs.Where(IsValidInput).Sum(input => input.ExpectedResult);

            return new ValueTask<string>(result.ToString());
        }

        private bool IsValidInput(EquationInput input)
        {
            List<List<Operator>> operatorCombinations = GetAllOperatorCombinations(input.Values.Length - 1);

            foreach (List<Operator> combination in operatorCombinations)
            {
                if (input.Evaluate(combination.ToArray()))
                    return true;
            }

            return false;
        }

        private List<List<Operator>> GetAllOperatorCombinations(int length)
        {
            if (_acceptedOperators == null)
                return new List<List<Operator>>();

            var result = new List<List<Operator>> { new List<Operator>() };

            for (var i = 0; i < length; i++)
            {
                var newCombinations = new List<List<Operator>>();

                foreach (List<Operator> combination in result)
                {
                    foreach (Operator possibleOperator in _acceptedOperators)
                    {
                        var newCombination = new List<Operator>(combination) { possibleOperator };
                        newCombinations.Add(newCombination);
                    }
                }

                result = newCombinations;
            }

            return result;
        }
    }

    public readonly struct EquationInput
    {
        public readonly long[] Values;
        public readonly long ExpectedResult;

        public EquationInput(long[] values, long expectedResult)
        {
            Values = values;
            ExpectedResult = expectedResult;
        }

        public bool Evaluate(Operator[] operators)
        {
            if (operators.Length < Values.Length - 1)
                throw new ArgumentException("Not enough operators!");

            long evaluation = Values[0];
            for (var i = 1; i < Values.Length; i++)
            {
                switch (operators[i - 1])
                {
                    case Operator.Add:
                        evaluation += Values[i];
                        break;
                    case Operator.Multiply:
                        evaluation *= Values[i];
                        break;
                    case Operator.Concatenate:
                        evaluation = long.Parse($"{evaluation}{Values[i]}");
                        break;
                }
            }

            return evaluation == ExpectedResult;
        }
    }

    public enum Operator
    {
        Add,
        Multiply,
        Concatenate,
    }
}
