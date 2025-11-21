namespace AdventOfCode
{
    using System.Collections.Generic;
    using System.Linq;
    using Point = GridDirectionUtils.Point;

    public class Day_08 : BaseDay
    {
        private readonly GridUtils<char> puzzleGrid;

        public Day_08()
        {
            puzzleGrid = GridUtils<char>.CreateCharGrid(InputFilePath);
            LogUtils.DebugLogMode = true;
        }

        public override ValueTask<string> Solve_1() => new(Solve_1_Synchron());
        public override ValueTask<string> Solve_2() => new(Solve_2_Synchron());

        private string Solve_1_Synchron()
        {
            var antinodes = new HashSet<Point>();
            foreach (var kvp in GetAntennaTypePositions(puzzleGrid))
            {
                CalculateAntinodes(kvp.Value, antinodes, puzzleGrid);
            }
            return antinodes.Count.ToString();
        }

        private string Solve_2_Synchron()
        {
            var antinodes = new HashSet<Point>();
            foreach (var kvp in GetAntennaTypePositions(puzzleGrid))
            {
                foreach (var antenna in kvp.Value)
                {
                    antinodes.Add(antenna);
                }
                CalculateAntinodes(kvp.Value, antinodes, puzzleGrid, part2: true);
            }
            return antinodes.Count.ToString();
        }

        private Dictionary<char, List<Point>> GetAntennaTypePositions(GridUtils<char> grid)
        {
            var antennaTypePositions = new Dictionary<char, List<Point>>();
            for (int y = 0; y < grid.RowCount; y++)
            {
                for (int x = 0; x < grid.ColumnCount; x++)
                {
                    var c = grid[y, x];
                    if (c == '.') continue;
                    if (!antennaTypePositions.TryGetValue(c, out var list))
                    {
                        list = new List<Point>();
                        antennaTypePositions[c] = list;
                    }
                    list.Add(new Point(y, x));
                }
            }
            return antennaTypePositions;
        }

        private void CalculateAntinodes(List<Point> positions, HashSet<Point> antinodes, GridUtils<char> grid, bool part2 = false)
        {
            if (positions.Count <= 1) return;
            var start = positions[0];
            foreach (var pos in positions.Skip(1))
            {
                CalculateDistance(start, pos, antinodes, grid, part2);
            }
            CalculateAntinodes(positions.Skip(1).ToList(), antinodes, grid, part2);
        }

        private void CalculateDistance(Point start, Point pos, HashSet<Point> antinodes, GridUtils<char> grid, bool part2 = false)
        {
            int diffY = pos.Y - start.Y;
            int diffX = pos.X - start.X;
            var initialCandidates = new[]
            {
                new Point(start.Y - diffY, start.X - diffX), // opposite direction of start→pos
                new Point(pos.Y + diffY, pos.X + diffX)      // same direction beyond pos
            };

            foreach (var initial in initialCandidates)
            {
                if (part2)
                {
                    // Determine the stepping direction based on the candidate
                    int dirY, dirX;
                    if (initial.Y < start.Y) // moving away from start
                    {
                        dirY = -diffY;
                        dirX = -diffX;
                    }
                    else // moving further in the same direction as start→pos
                    {
                        dirY = diffY;
                        dirX = diffX;
                    }

                    var candidate = initial;
                    while (AddAntinodeCandidate(candidate, antinodes, grid))
                    {
                        // Move one step further along the line
                        candidate = new Point(candidate.Y + dirY, candidate.X + dirX);
                    }
                }
                else
                {
                    AddAntinodeCandidate(initial, antinodes, grid);
                }
            }
        }

        private bool AddAntinodeCandidate(Point antinodeCandidate, HashSet<Point> antinodes, GridUtils<char> grid, bool part2 = false)
        {
            if (!grid.IsInBounds(antinodeCandidate.Y, antinodeCandidate.X)) return false;
            antinodes.Add(antinodeCandidate);
            return true;
        }
    }
}