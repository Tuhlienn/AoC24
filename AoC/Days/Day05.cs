namespace AoC
{
    public sealed class Day05 : BaseDay
    {
        private readonly Dictionary<int, List<int>> _orderingRules = new();
        private readonly List<int[]> _updates = new();

        public Day05()
        {
            string[] lines = File.ReadAllLines(InputFilePath);

            var readingRules = true;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    readingRules = false;
                    continue;
                }

                if (readingRules)
                {
                    int[] rules = line.Split('|').Select(int.Parse).ToArray();
                    if (!_orderingRules.ContainsKey(rules[0]))
                        _orderingRules.Add(rules[0], new List<int>());
                    _orderingRules[rules[0]].Add(rules[1]);
                }
                else
                {
                    int[] pages = line.Split(',').Select(int.Parse).ToArray();
                    _updates.Add(pages);
                }
            }
        }

        public override ValueTask<string> Solve_1()
        {
            var result = 0;

            foreach (int[] update in _updates)
            {
                if (!IsValidUpdate(update))
                    continue;

                result += GetMiddleNumber(update);
            }

            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var result = 0;

            foreach (int[] update in _updates)
            {
                if (IsValidUpdate(update))
                    continue;

                int[] correctedUpdate = CorrectUpdate(update);
                result += GetMiddleNumber(correctedUpdate);
            }

            return new ValueTask<string>(result.ToString());
        }

        private bool IsValidUpdate(int[] update)
        {
            var alreadyVisited = new HashSet<int>();

            foreach (int page in update)
            {
                alreadyVisited.Add(page);

                if (!_orderingRules.TryGetValue(page, out List<int>? rules))
                    continue;

                if (rules.Any(rule => alreadyVisited.Contains(rule)))
                    return false;
            }

            return true;
        }

        private int[] CorrectUpdate(int[] update)
        {
            List<(int from, int to)> applyingRules = GetApplyingRules(update);

            for (var i = 0; i < update.Length; i++)
            {
                int page1 = update[i];
                for (int j = i + 1; j < update.Length; j++)
                {
                    int page2 = update[j];
                    if (!applyingRules.Any(rule => rule.to == page1 && rule.from == page2))
                        continue;

                    update[i] = page2;
                    update[j] = page1;
                    page1 = page2;
                }
            }

            return update;
        }

        private List<(int from, int to)> GetApplyingRules(int[] update)
        {
            List<(int from, int to)> appliedRules = new();

            foreach (KeyValuePair<int, List<int>> rule in _orderingRules.Where(rule => update.Contains(rule.Key)))
                appliedRules.AddRange(rule.Value.Where(update.Contains).Select(page => (rule.Key, page)));

            return appliedRules;
        }

        private static int GetMiddleNumber(int[] update)
        {
            int index = update.Length / 2;
            return update[index];
        }
    }
}
