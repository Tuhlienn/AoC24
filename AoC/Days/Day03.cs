using System.Text.RegularExpressions;

namespace AoC
{
    public sealed partial class Day03 : BaseDay
    {
        private readonly string _input;

        public Day03()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            MatchCollection matches = MulInstructionRegex().Matches(_input);
            var result = 0;
            foreach (Match match in matches)
            {
                result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }

            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            MatchCollection matches = ExtendedMulInstructionRegex().Matches(_input);
            var result = 0;
            var enabled = true;
            foreach (Match match in matches)
            {
                if (match.Groups[1].Value == "do")
                    enabled = true;
                else if (match.Groups[2].Value == "don't")
                    enabled = false;
                else if (enabled)
                    result += int.Parse(match.Groups[3].Value) * int.Parse(match.Groups[4].Value);
            }

            return new ValueTask<string>(result.ToString());
        }

        [GeneratedRegex(@"mul\(([0-9]{1,3}),([0-9]{1,3})\)")]
        private static partial Regex MulInstructionRegex();

        [GeneratedRegex(@"(do)\(\)|(don't)\(\)|mul\(([0-9]{1,3}),([0-9]{1,3})\)")]
        private static partial Regex ExtendedMulInstructionRegex();
    }
}
