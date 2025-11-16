# Advent Of Code

## Usage

Copy the folder and rename it (i.e. to the year).

Create a file `Day_XY.cs` in `Days` for the day you are working on based on the `_Day_Template._cs` file and solve the puzzle (fill days with leading zeros)

Create a file `XY.txt` in `Inputs` and copy your AOC input for the day into it

### Utils Usage

in `AdventOfCode.csproj` file you might need to adjust the Utils include path `<ProjectReference Include="..\Utils\Utils.csproj" />`

classes from Utils can directly be used with `<ClassName>.<FunctionName>` as `Utils` namespace is used globally

## Execute

- `dotnet run` to run the latest day that exist in your files
- `dotnet run XY` to run a specific day
- `dotnet run all` to run all days

## Credits

based on <https://github.com/eduherminio/AdventOfCode.Template>
