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

			var treeGrid = puzzleInputRaw.Select(i => i.ToCharArray()).ToList();

			Console.WriteLine($"* Tree grid is {treeGrid[0].Length:N0} x {treeGrid.Count:N0} trees");
            Console.WriteLine("* Tree grid:");
            for (int r = 0; r < treeGrid.Count; r++)
            {
                Console.WriteLine($"** {string.Join("", treeGrid[r])}");
            }

            PartA(treeGrid);
            PartB(treeGrid);
        }

        private static void PartA(List<char[]> treeGrid)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

			var treeVisibility = new bool[treeGrid[0].Length, treeGrid.Count];

			// all trees on the border of the grid are visible
			// mark all trees on the top and bottom (north and south) rows as visible
			for (int i = 0; i < treeGrid[0].Length; i++)
			{
				treeVisibility[i, 0] = true;
				treeVisibility[i, treeGrid.Count - 1] = true;
			}
			
			// mark all trees on the left and right (west and east) columns as visible
			for (int i = 1; i < treeGrid.Count - 1; i++)
			{
				treeVisibility[0, i] = true;
                treeVisibility[treeGrid[0].Length - 1, i] = true;
			}

            for (int r = 1; r < treeGrid.Count - 1; r++)
            {
                for (int c = 1; c < treeGrid[0].Length - 1; c++)
                {
                    var currentTree = treeGrid[r][c];

                    if (treeVisibility[c, r])
                        continue;

                    // check visibility to west
                    var treeIsVisble = true;
                    for (int vw = c - 1; vw >= 0; vw--)
                    {
                        treeIsVisble = treeGrid[r][vw] < currentTree;
                        
                        if (!treeIsVisble)
                            break;
                    }

                    if (treeIsVisble)
                    {
                        treeVisibility[c, r] = treeIsVisble;
                        continue;
                    }

                    // check visibility to east
                    treeIsVisble = true;
                    for (int ve = c + 1; ve < treeGrid[0].Length; ve++)
                    {
                        treeIsVisble = treeGrid[r][ve] < currentTree;

                        if (!treeIsVisble)
                            break;
                    }

                    if (treeIsVisble)
                    {
                        treeVisibility[c, r] = treeIsVisble;
                        continue;
                    }

                    // check visibility to north
                    treeIsVisble = true;
                    for (int vn = r - 1; vn >= 0; vn--)
                    {
                        treeIsVisble = treeGrid[vn][c] < currentTree;

                        if (!treeIsVisble)
                            break;
                    }

                    if (treeIsVisble)
                    {
                        treeVisibility[c, r] = treeIsVisble;
                        continue;
                    }

                    // check visibility to south
                    treeIsVisble = true;
                    for (int vs = r + 1; vs < treeGrid.Count; vs++)
                    {
                        treeIsVisble = treeGrid[vs][c] < currentTree;

                        if (!treeIsVisble)
                            break;
                    }

                    if (treeIsVisble)
                    {
                        treeVisibility[c, r] = treeIsVisble;
                        continue;
                    }
                }
            }

			Console.WriteLine("\r\n* Visibility");
			var visibleTrees = 0;
			for (int c = 0; c < treeGrid[0].Length; c++)
			{
				var line = string.Empty;
				
				for (int r = 0; r < treeGrid.Count; r++)
				{
					visibleTrees += treeVisibility[c, r] ? 1 : 0;
					line += treeVisibility[c, r] ? "*" : " ";
				}

				Console.WriteLine($"** {line}");
			}

			Console.WriteLine($"*** Number of visible trees: {visibleTrees:N0}");
        }

        private static void PartB(List<char[]> treeGrid)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            var scenicScores = new List<int>();

            for (int r = 0; r < treeGrid.Count; r++)
            {
                for (int c = 0; c < treeGrid[0].Length; c++)
                {
                    var currentTree = treeGrid[r][c];
                    var treesToWest = 0;
                    var treesToEast = 0;
                    var treesToNorth = 0;
                    var treesToSouth = 0;

                    // calculate how far west we can see
                    for (int vw = c - 1; vw >= 0; vw--)
                    {
                        if (vw >= 0)
                        {
                            treesToWest++;
                            if (treeGrid[r][vw] >= currentTree)
                                break;
                        }
                    }

                    // calculate how far east we can see
                    for (int ve = c + 1; ve < treeGrid[0].Length; ve++)
                    {
                        if (ve < treeGrid[0].Length)
                        {
                            treesToEast++;
                            if (treeGrid[r][ve] >= currentTree)
                                break;
                        }
                    }

                    // calculate how far north we can see
                    for (int vn = r - 1; vn >= 0; vn--)
                    {
                        if (vn >= 0)
                        {
                            treesToNorth++;
                            if (treeGrid[vn][c] >= currentTree)
                                break;
                        }
                    }

                    // calculate how far south we can see
                    for (int vs = r + 1; vs < treeGrid.Count; vs++)
                    {
                        if (vs < treeGrid.Count)
                        {
                            treesToSouth++;
                            if (treeGrid[vs][c] >= currentTree)
                                break;
                        }
                    }

                    scenicScores.Add(treesToWest * treesToEast * treesToNorth * treesToSouth);
                }
            }

            var highestScenicScore = scenicScores.Max();
            Console.WriteLine($"*** Highest scenic score: {highestScenicScore:N0}");
        }
    }
}