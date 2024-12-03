namespace AoC
{
    public sealed class Day01 : BaseDay
    {
        private readonly List<int> _listA;
        private readonly List<int> _listB;

        public Day01()
        {
            using var reader = new StreamReader(InputFilePath);

            _listA = new List<int>();
            _listB = new List<int>();
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line == null)
                    continue;

                string[] values = line.Split(' ');
                _listA.Add(int.Parse(values[0]));
                _listB.Add(int.Parse(values[1]));
            }
        }

        public override ValueTask<string> Solve_1()
        {
            var totalDiff = 0;
            for (var i = 0; i < _listA.Count && i < _listB.Count; i++)
            {
                totalDiff += Math.Abs(_listA[i] - _listB[i]);
            }

            return new ValueTask<string>(totalDiff.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var result = 0;
            foreach (int number in _listA)
            {
                int appearances = _listB.Count(x => x == number);
                result += appearances * number;
            }

            return new ValueTask<string>(result.ToString());
        }
    }
}
