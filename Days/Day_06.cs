namespace AdventOfCode
{
    using Direction = GridDirectionUtils.Direction;
    using Point = GridDirectionUtils.Point;
    // Small immutable record for a position that includes direction
    readonly record struct Position(Point P, Direction D);

    // Day_06 class which inherits from BaseDay and contains the Solve functions for the puzzle
    public class Day_06 : BaseDay
    {
        // Define class variables
        private readonly GridUtils<char> puzzleGrid;
        // Mapping for turning right
        static readonly Dictionary<Direction, Direction> TurnRight = new()
        {
            { Direction.Up, Direction.Right },
            { Direction.Right, Direction.Down },
            { Direction.Down, Direction.Left },
            { Direction.Left, Direction.Up }
        };

        // Constructor
        public Day_06()
        {
            // Initialize class variables
            // Parse the input file here, to avoid diluting the puzzle solution times
            puzzleGrid = GridUtils<char>.CreateCharGrid(InputFilePath);

            // TODO enable/disable when needed
            // LogUtils.DebugLogMode = true;
            // LogUtils.Debug($"DebugLogMode is {LogUtils.DebugLogMode}");
        }

        // Solve functions asynchron (called from Program.cs)
        public override ValueTask<string> Solve_1() => new(Solve_1_Synchron());
        public override ValueTask<string> Solve_2() => new(Solve_2_Synchron());

        // Synchron implementation to receive solution for puzzle part1
        private string Solve_1_Synchron()
        {
            var start = GetStartLocation(puzzleGrid);
            LogUtils.Debug(string.Join(", ", start));
            var visitedLocations = GetVisitedLocations(start, puzzleGrid);
            return visitedLocations.Count.ToString();
        }

        // Synchron implementation to receive solution for puzzle part2
        private string Solve_2_Synchron()
        {
            var start = GetStartLocation(puzzleGrid);
            LogUtils.Debug(string.Join(", ", start));
            HashSet<Point> guardPath = GetVisitedLocations(start, puzzleGrid);
            var loopingLoops = guardPath.Count(guardPos => IsLoopingLoop(puzzleGrid, guardPos, start));
            return loopingLoops.ToString();
        }

        private static Position GetStartLocation(GridUtils<char> puzzleGrid)
        {
            for (int y = 0; y < puzzleGrid.RowCount; y++)
            {
                for (int x = 0; x < puzzleGrid.ColumnCount; x++)
                {
                    if (puzzleGrid[y, x] == '^')
                        return new Position(new Point(y, x), Direction.Up);
                }
            }
            throw new InvalidOperationException("Start position '^' not found in the grid.");
        }

        private HashSet<Point> GetVisitedLocations(Position guardPosition, GridUtils<char> puzzleGrid)
        {
            var visitedLocations = new HashSet<Point>();
            while (true)
            {
                visitedLocations.Add(guardPosition.P);
      
                var nextGuardPosition = GetNextPosition(guardPosition, puzzleGrid);
                if (nextGuardPosition == guardPosition) break;
                guardPosition = nextGuardPosition;                
            }
            return visitedLocations;
        }

        private Position GetNextPosition(Position guardPosition, GridUtils<char> puzzleGrid, Point? blocked = null)
        {
            // Retrieve the direction vector from GridDirectionUtils
            var dirVector = GridDirectionUtils.Directions[guardPosition.D];
            Point nextPoint = new Point(guardPosition.P.Y + dirVector.dy, guardPosition.P.X + dirVector.dx);
            Direction nextDir = guardPosition.D;
            if (!puzzleGrid.IsInBounds(nextPoint.Y, nextPoint.X))
            {
                return guardPosition;
            }
            if (puzzleGrid[nextPoint.Y, nextPoint.X] == '#' || blocked == nextPoint)
            {
                nextDir = TurnRight[guardPosition.D];
                nextPoint = guardPosition.P;
            }
            return new Position(nextPoint, nextDir);
        }

        private bool IsLoopingLoop(GridUtils<char> puzzleGrid, Point blockedPoint, Position guardPosition)
        {
            var visitedLocations = new HashSet<Position>();
            while (true)
            {
                if (visitedLocations.Contains(guardPosition)) return true;

                visitedLocations.Add(guardPosition);

                var nextGuardPosition = GetNextPosition(guardPosition, puzzleGrid, blockedPoint);
                if (nextGuardPosition == guardPosition) return false;
                guardPosition = nextGuardPosition;
            }
        }
    }
}
