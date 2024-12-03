namespace AoC
{
    public sealed class Day02 : BaseDay
    {
        private readonly List<List<int>> _reports;

        public Day02()
        {
            using var reader = new StreamReader(InputFilePath);

            var result = new List<List<int>>();
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line == null)
                    continue;

                string[] values = line.Split(' ');
                result.Add(values.Select(int.Parse).ToList());
            }

            _reports = result;
        }

        public override ValueTask<string> Solve_1()
        {
            var safeReports = 0;
            foreach (List<int> report in _reports)
            {
                if (CheckReportSafety(report))
                    safeReports++;
            }

            return new ValueTask<string>(safeReports.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var safeReports = 0;
            foreach (List<int> report in _reports)
            {
                if (CheckReportSafety(report))
                {
                    safeReports++;
                    continue;
                }

                for (var i = 0; i < report.Count; i++)
                {
                    List<int> reduced = report.Where((v, index) => index != i).ToList();
                    if (!CheckReportSafety(reduced))
                        continue;

                    safeReports++;
                    break;
                }
            }

            return new ValueTask<string>(safeReports.ToString());
        }

        private static bool CheckReportSafety(List<int> report)
        {
            bool ascending = report[0] < report[1];
            for (var i = 0; i < report.Count - 1; i++)
            {
                int diff = report[i + 1] - report[i];

                if (CheckSafety(diff, ascending))
                    return false;
            }

            return true;
        }

        private static bool CheckSafety(int diff, bool ascending)
        {
            return diff == 0 || diff > 0 != ascending || Math.Abs(diff) > 3;
        }
    }
}
