using System;
using System.IO;
using System.Linq;

namespace Day08
{
    // Puzzle description: https://adventofcode.com/2022/day/8

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 8");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			var gridHorizontal = puzzleInputRaw.Select(i => i.ToCharArray()).ToList();
			var gridVertical = CalculateVerticalGrid(gridHorizontal);

			Console.WriteLine($"* Tree grid is {gridHorizontal[0].Length:N0} x {gridHorizontal.Count:N0} trees");

			PartA(gridHorizontal, gridVertical);
            PartB();
        }

		private static List<char[]> CalculateVerticalGrid(List<char[]> grid)
		{
			var gridVertical = new List<char[]>();

			for (int i = 0; i < grid[0].Length; i++)
			{
				var colArray = grid.Select(r => r[i]).ToArray();
				gridVertical.Add(colArray);
			}

			return gridVertical;
		}

        private static void PartA(List<char[]> gridHorizontal, List<char[]> gridVertical)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

			var treeVisibility = new bool[gridHorizontal[0].Length, gridVertical[0].Length];

			// all trees on the border of the grid are visible
			// var treesOnBorder = gridHorizontal[0].Length * 2 + (gridVertical[0].Length - 2) * 2;

			// mark all trees on the top and bottom (north and south) rows as visible
			for (int i = 0; i < gridHorizontal[0].Length; i++)
			{
				treeVisibility[i, 0] = true;
				treeVisibility[i, gridHorizontal.Count - 1] = true;
			}
			
			// mark all trees on the left and right (west and east) columns as visible
			for (int i = 1; i < gridVertical[0].Length - 1; i++)
			{
				treeVisibility[0, i] = true;
				treeVisibility[gridVertical.Count - 1, i] = true;
			}

			Console.WriteLine("* Normal tree grid:");
			for (int r = 0; r < gridHorizontal.Count; r++)
			{
				Console.WriteLine($"** {string.Join("", gridHorizontal[r])}");
			}

			Console.WriteLine("\r\n* Rotated tree grid:");
			for (int r = 0; r < gridVertical.Count; r++)
			{
				Console.WriteLine($"** {string.Join("", gridVertical[r])}");
			}

			// process the grid west/east, skipping the border trees
			for (int c = 1; c < gridHorizontal[0].Length - 1; c++)
			{
				for (int r = 1; r < gridVertical[0].Length - 1; r++)
				{
					if (treeVisibility[c, r])
						continue;
					
					var treeHeight = gridHorizontal[r][c];
					
					// is the tree visible from the west?
					if (!(gridHorizontal[r][..(c - 1)].Count(h => h >= treeHeight) > 0))
					{
						treeVisibility[c, r] = true;
						continue;
					}

					// is the tree visible from the east?
					if (!(gridHorizontal[r][(c + 1)..].Count(h => h >= treeHeight) > 0))
					{
						treeVisibility[c, r] = true;
						continue;
					}
				}
			}

			if (1 == 0)
			{

				// process the grid north/south, skipping the border trees
				for (int c = 1; c < gridVertical[0].Length - 1; c++)
				{
					for (int r = 1; r < gridHorizontal[0].Length - 1; r++)
					{
						if (treeVisibility[r, c])
							continue;

						var treeHeight = gridVertical[r][c];

						// is the tree visible from the north?
						if (!(gridVertical[r][..(c - 1)].Count(h => h >= treeHeight) > 0))
						{
							treeVisibility[r, c] = true;
							continue;
						}

						// is the tree visible from the south?
						if (!(gridVertical[r][(c + 1)..].Count(h => h >= treeHeight) > 0))
						{
							treeVisibility[r, c] = true;
							continue;
						}
					}
				}

			}

			Console.WriteLine("\r\n* Visibility");
			var visibleTrees = 0;
			for (int c = 0; c < gridHorizontal[0].Length; c++)
			{
				var line = string.Empty;
				
				for (int r = 0; r < gridVertical[0].Length; r++)
				{
					visibleTrees += treeVisibility[c, r] ? 1 : 0;
					line += treeVisibility[c, r] ? "*" : " ";
				}

				Console.WriteLine($"** {line}");
			}

			Console.WriteLine($"*** Number of visible trees: {visibleTrees:N0}");
        }

        private static void PartB()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");
        }
    }
}