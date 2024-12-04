namespace AoC
{
    public sealed class Day04 : BaseDay
    {
        private readonly char[][] _input;

        private readonly (int x, int y)[] _allDirections = {
            new (1, 0),
            new (1, 1),
            new (0, 1),
            new (-1, 1),
            new (-1, 0),
            new (-1, -1),
            new (0, -1),
            new (1, -1),
        };

        private readonly (int x, int y)[] _diagonalDirections = {
            new (1, 1),
            new (-1, 1),
            new (-1, -1),
            new (1, -1),
        };

        private const string XmasWord = "XMAS";
        private const string MasWord = "MAS";

        public Day04()
        {
            string[] lines = File.ReadAllLines(InputFilePath);
            _input = lines.Select(line => line.ToCharArray()).ToArray();
        }

        public override ValueTask<string> Solve_1()
        {
            var matches = 0;
            for (var y = 0; y < _input.Length; y++)
            {
                for (var x = 0; x < _input[y].Length; x++)
                {
                    if (_input[y][x] != XmasWord[0])
                        continue;

                    matches += CountXmasMatches(x, y);
                }
            }

            return new ValueTask<string>(matches.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var matches = 0;
            for (var y = 0; y < _input.Length; y++)
            {
                for (var x = 0; x < _input[y].Length; x++)
                {
                    if (_input[y][x] != MasWord[1])
                        continue;

                    if (IsCrossMasMatchAtCenter(x, y))
                        matches++;
                }
            }

            return new ValueTask<string>(matches.ToString());
        }

        private int CountXmasMatches(int x, int y)
        {
            var matches = 0;
            foreach ((int xDirection, int yDirection) in _allDirections)
            {
                var failed = false;
                for (var i = 0; i < XmasWord.Length; i++)
                {
                    int xPos = x + xDirection * i;
                    int yPos = y + yDirection * i;
                    if (xPos < 0 || xPos >= _input[0].Length || yPos < 0 || yPos >= _input.Length)
                    {
                        failed = true;
                        break;
                    }

                    char currentChar = _input[yPos][xPos];
                    if (currentChar == XmasWord[i])
                        continue;

                    failed = true;
                    break;
                }

                if (!failed)
                    matches++;
            }

            return matches;
        }

        private bool IsCrossMasMatchAtCenter(int x, int y)
        {
            if (x <= 0 || x >= _input[0].Length - 1 || y <= 0 || y >= _input.Length - 1)
                return false;

            char upperLeft = _input[y - 1][x - 1];
            char lowerRight = _input[y + 1][x + 1];
            if (upperLeft == lowerRight || !IsSOrM(upperLeft) || !IsSOrM(lowerRight))
                return false;

            char upperRight = _input[y - 1][x + 1];
            char lowerLeft = _input[y + 1][x - 1];
            if (upperRight == lowerLeft || !IsSOrM(upperRight) || !IsSOrM(lowerLeft))
                return false;

            return true;
        }

        private static bool IsSOrM(char upperLeft)
        {
            return upperLeft == MasWord[0] || upperLeft == MasWord[2];
        }
    }
}
