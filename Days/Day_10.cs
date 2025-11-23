namespace AdventOfCode
{
    using Point = GridDirectionUtils.Point;
    // Day_10 class which inherits from BaseDay and contains the Solve functions for the puzzle
    public class Day_10 : BaseDay
    {
        // TODO:  comment to use real puzzle data, comment out to use example puzzle data
        // protected override string InputFileDirPath => "InputsExample";

        // Define class variables
        private readonly GridUtils<int> _puzzleGrid;

        // Constructor
        public Day_10()
        {
            // Initialize class variables
            // Parse the input file here, to avoid deluting the puzzle solution times
            _puzzleGrid = GridUtils<int>.CreateIntGrid(InputFilePath);

            // TODO enable/disable when needed
            // LogUtils.DebugLogMode = true;
            // LogUtils.Debug($"DebugLogMode is {LogUtils.DebugLogMode}");
        }

        // Solve functions asynchron (called from Program.cs)
        public override ValueTask<string> Solve_1() => new(Solve_1_Synchron());
        public override ValueTask<string> Solve_2() => new(Solve_2_Synchron());

        // Function to parse the puzzle input into the required data structure to solve the puzzle
        public static string ParseInput(string InputFilePath)
        {
            return File.ReadAllText(InputFilePath);
        }

        // Synchron implementation to receive solution for puzzle part1
        private string Solve_1_Synchron()
        {
            var trailheads = FindTrailheads(_puzzleGrid);
            var summitCount = 0;
            foreach (var trailhead in trailheads)
            {
                HashSet<Point> summits = new();
                FindReachableSummitsFromTrailhead(trailhead, _puzzleGrid, summits);
                summitCount += summits.Count;
            }
            return summitCount.ToString();
        }

        // Synchron implementation to receive solution for puzzle part2
        private string Solve_2_Synchron()
        {
            var trailheads = FindTrailheads(_puzzleGrid);
            // var pathCount = 0;
            List<HashSet<Point>> paths = new();
            foreach (var trailhead in trailheads)            
                FindPathsToSummits(trailhead, _puzzleGrid, paths);
            return paths.Count.ToString();
        }

        private HashSet<Point> FindTrailheads(GridUtils<int> grid)
        {
            HashSet<Point> trailheads = new();
            for (int y = 0; y < grid.RowCount; y++)
                for (int x = 0; x < grid.ColumnCount; x++)
                    if (grid[y, x] == 0)
                    {
                        trailheads.Add(new Point(y, x));
                        LogUtils.Debug($"trailhead found: {(y, x)}");
                    }
            return trailheads;
        }

        private void FindReachableSummitsFromTrailhead(Point startPoint, GridUtils<int> grid, HashSet<Point> summits)
        {
            if (grid[startPoint] == 9)
            {
                summits.Add(startPoint);
                LogUtils.Debug($"reached summit {(startPoint.Y, startPoint.X)}");
            }
            else
                foreach (var neighbor in grid.GetStraightNeighbors(startPoint))
                    if (grid[startPoint] + 1 == neighbor.value)                    
                        FindReachableSummitsFromTrailhead(neighbor.p, grid, summits);                    
        }
        
        private void FindPathsToSummits(Point startPoint, GridUtils<int> grid, List<HashSet<Point>> paths, HashSet<Point> currentPath=null)
        {
            if (currentPath is null)            
                currentPath = new HashSet<Point>();
            currentPath.Add(startPoint);
            
            if (grid[startPoint] == 9)
            {
                paths.Add(currentPath);
                LogUtils.Debug($"reached summit {(startPoint.Y, startPoint.X)}");
            }
            else
                foreach (var neighbor in grid.GetStraightNeighbors(startPoint))                
                    if (grid[startPoint] + 1 == neighbor.value)                    
                        FindPathsToSummits(neighbor.p, grid, paths, currentPath);             
        }
    }
}
