namespace AdventOfCode;

// Day_04 class which inherits from BaseDay and contains the Solve functions for the puzzle
public class Day_04 : BaseDay
{
    // TODO:  comment to use real puzzle data, comment out to use example puzzle data
    // protected override string InputFileDirPath => "InputsExample";

    // Define class variables
    private GridUtils<char> puzzleGrid;

    // Constructor
    public Day_04()
    {
        // Initialize class variables
        // Parse the input file here, to avoid deluting the puzzle solution times
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
        return findWordOccurences(puzzleGrid, "XMAS").ToString();
    }

    // Synchron implementation to receive solution for puzzle part2
    private string Solve_2_Synchron()
    {
        // How many times is the word MAS in a X shape in the grid
        // M - M    M - S
        // - A -    - A -
        // S - S    M - S   
        return findWordOccurencesInXForm(puzzleGrid, "MAS").ToString();
    }
    
    private int findWordOccurences(GridUtils<char> grid, ReadOnlySpan<char> word)
    {
        var occurences = 0;
        for (int sy = 0; sy < grid.RowCount; sy++)
        {
            for (int sx = 0; sx < puzzleGrid.ColCount; sx++)
            {
                if (grid[sy, sx] != word[0]) continue;                
                foreach (var direction in GridDirectionUtils.Directions.Values)
                {
                    var i = 1;
                    var y = sy + direction.dy;
                    var x = sx + direction.dx;
                    while (i < word.Length && grid.IsInBounds(y, x) && grid[y, x] == word[i])
                    {
                        y += direction.dy;
                        x += direction.dx;
                        i++;
                    }
                    if (i == word.Length) occurences++;                        
                }                
            }
        }        
        return occurences;
    }
    
    private int findWordOccurencesInXForm(GridUtils<char> grid, ReadOnlySpan<char> word)
    {
        if (word.Length % 2 == 0 || word.Length == 1) throw new ArgumentException("Word must have odd length > 1");

        int occurences = 0;
        int halfWordLength = word.Length / 2;
        char middleLetter = word[halfWordLength];

        // Define the two pairs of opposite diagonal directions for the 'X' shape
        // Diagonal 1: up-left <-> down-right
        var upLeftDir = GridDirectionUtils.DiagonalDirections["up-left"];
        var downRightDir = GridDirectionUtils.DiagonalDirections["down-right"];

        // Diagonal 2: up-right <-> down-left
        var downLeftDir = GridDirectionUtils.DiagonalDirections["down-left"];
        var upRightDir = GridDirectionUtils.DiagonalDirections["up-right"];
        
        for (int y = 0; y < grid.RowCount; y++)
        {
            for (int x = 0; x < grid.ColCount; x++)
            {
                if (grid[y, x] != middleLetter) continue;
                // Check the first diagonal (up-left to down-right)
                bool diagonal1Match = CheckXDiagonalSegment(grid, y, x, upLeftDir, downRightDir, word, halfWordLength);
                // Check the second diagonal (up-right to down-left)
                bool diagonal2Match = CheckXDiagonalSegment(grid, y, x, downLeftDir, upRightDir, word, halfWordLength);
                if (diagonal1Match && diagonal2Match) occurences++;
            }
        }
        return occurences;
    }

    // Helper to check if a full diagonal segment of the X pattern matches the word
    private bool CheckXDiagonalSegment(GridUtils<char> grid, int centerY, int centerX,
                                        (int dy, int dx) leftDir,
                                        (int dy, int dx) rightDir,
                                        ReadOnlySpan<char> word, int halfWordLength)
    {
        bool forwardWordMatch = true;
        bool reverseWordMatch = true;

        for (int i = 1; i <= halfWordLength; i++)
        {
            // Calculate coordinates for segment 1 (e.g., up-left from center)
            int leftY = centerY + leftDir.dy * i;
            int leftX = centerX + leftDir.dx * i;

            // Calculate coordinates for segment 2 (e.g., down-right from center)
            int rightY = centerY + rightDir.dy * i;
            int rightX = centerX + rightDir.dx * i;

            // Check bounds for both points
            if (!grid.IsInBounds(leftY, leftX) || !grid.IsInBounds(rightY, rightX))
            {
                forwardWordMatch = reverseWordMatch = false;
                break; // If any point is out of bounds, neither pattern can match
            }
            if (forwardWordMatch && (grid[leftY, leftX] != word[halfWordLength - i] || grid[rightY, rightX] != word[halfWordLength + i]))
            {
                forwardWordMatch = false;
            }
            if (reverseWordMatch && (grid[leftY, leftX] != word[halfWordLength + i] || grid[rightY, rightX] != word[halfWordLength - i]))
            {
                reverseWordMatch = false;
            }
            // If both patterns have failed, no need to check further along this diagonal
            if (!forwardWordMatch && !reverseWordMatch) break;
        }
        return forwardWordMatch || reverseWordMatch;
    }
}
