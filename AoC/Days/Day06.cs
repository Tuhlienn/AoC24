namespace AoC
{
    public sealed class Day06 : BaseDay
    {
        private readonly char[][] _input;
        private readonly int _ySize;
        private readonly int _xSize;

        private readonly (int x, int y)[] _directions =
        {
            (-1, 0),
            (0, 1),
            (1, 0),
            (0, -1)
        };

        public Day06()
        {
            string[] lines = File.ReadAllLines(InputFilePath);
            _input = lines.Select(line => line.ToCharArray()).ToArray();

            _xSize = _input.Length;
            _ySize = _input[0].Length;
        }

        public override ValueTask<string> Solve_1()
        {
            (int x, int y) startPos = FindStartPosition();

            HashSet<(int x, int y)> visits = TraverseMapFromPosition(startPos);

            return new ValueTask<string>(visits.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            (int x, int y) startPos = FindStartPosition();

            HashSet<(int x, int y)> visits = TraverseMapFromPosition(startPos);

            var loopBlocks = new HashSet<(int x, int y)>();
            foreach ((int x, int y) visit in visits)
            {
                if (visit == startPos)
                    continue;

                char[][] map = _input.Select(a => a.ToArray()).ToArray();
                map[visit.x][visit.y] = '#';
                if (CheckForLoop(map, startPos))
                    loopBlocks.Add(visit);
            }

            return new ValueTask<string>(loopBlocks.Count.ToString());
        }

        private (int x, int y) FindStartPosition()
        {
            for (var y = 0; y < _ySize; y++)
            {
                for (var x = 0; x < _xSize; x++)
                {
                    if (_input[x][y] == '^')
                        return (x, y);
                }
            }

            throw new ArgumentException("Input has no start position");
        }

        private HashSet<(int x, int y)> TraverseMapFromPosition((int x, int y) startPos)
        {
            var visited = new HashSet<(int x, int y)>();

            (int x, int y) currentPos = startPos;
            var currentDirectionIndex = 0;

            while (true)
            {
                visited.Add(currentPos);

                (int deltaX, int deltaY) currentDirection = _directions[currentDirectionIndex];
                (int x, int y) nextPos = (currentPos.x + currentDirection.deltaX, currentPos.y + currentDirection.deltaY);
                if (!IsInsideMap(nextPos))
                    break;

                if (_input[nextPos.x][nextPos.y] != '#')
                {
                    currentPos = nextPos;
                    continue;
                }

                currentDirectionIndex = CycleDirection(currentDirectionIndex);
            }

            return visited;
        }

        private bool CheckForLoop(char[][] map, (int x, int y) startPos)
        {
            var hits = new Dictionary<(int x, int y), List<int>>();
            (int x, int y) currentPos = startPos;
            var currentDirectionIndex = 0;

            while (true)
            {
                (int deltaX, int deltaY) currentDirection = _directions[currentDirectionIndex];
                (int x, int y) nextPos = (currentPos.x + currentDirection.deltaX, currentPos.y + currentDirection.deltaY);
                if (!IsInsideMap(nextPos))
                    return false;

                if (map[nextPos.x][nextPos.y] != '#')
                {
                    currentPos = nextPos;
                    continue;
                }

                if (hits.TryGetValue(nextPos, out List<int>? hitDirections) && hitDirections.Contains(currentDirectionIndex))
                    return true;

                AddHit(hits, nextPos, currentDirectionIndex);

                currentDirectionIndex = CycleDirection(currentDirectionIndex);
            }
        }

        private int CycleDirection(int currentDirectionIndex)
        {
            return (currentDirectionIndex + 1) % _directions.Length;
        }

        private bool IsInsideMap((int x, int y) position)
        {
            return position.x >= 0 && position.x < _xSize && position.y >= 0 && position.y < _ySize;
        }

        private static void AddHit(Dictionary<(int x, int y), List<int>> hits, (int x, int y) nextPos, int directionIndex)
        {
            if (!hits.ContainsKey(nextPos))
                hits[nextPos] = new List<int>();

            hits[nextPos].Add(directionIndex);
        }
    }
}
