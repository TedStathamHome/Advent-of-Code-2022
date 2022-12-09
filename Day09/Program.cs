using System;
using System.IO;
using System.Linq;

namespace Day09
{
    // Puzzle description: https://adventofcode.com/2022/day/9

    class HeadMovements
    {
        public char DirectionToMove { get; set; }
        public int StepsToMove { get; set; }
    }

    class Program
    {
        private static bool[,] visitedGridPoints;

        private class HeadPos
        {
            public int x { get; set; } = 0;
            public int y { get; set; } = 0;
        }

        private class TailPos
        {
            public int x { get; set; } = 0;
            public int y { get; set; } = 0;
        }

        private static HeadPos headPos = new HeadPos();
        private static TailPos tailPos = new TailPos();

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 9");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();
            var headMovements = puzzleInputRaw.Select(i => new HeadMovements()
                {
                    DirectionToMove = i[0], 
                    StepsToMove = int.Parse(i[3..]) 
                }).ToList();

            Console.WriteLine($"* Head movements to process: {headMovements.Count:N0}");

            BuildGrid(headMovements);

            Console.WriteLine($"* Grid is {headPos.x * 2 + 1:N0} x {headPos.y * 2 + 1:N0}");

			PartA();
            PartB();
        }

        private static void BuildGrid(List<HeadMovements> headMovements)
        {
            // 1. figure out how far left/right or up/down the head moves
            // 2. the larger of these two values is what we need to move within horizontall/vertically
            // 3. the grid horizontal/vertical size needs to be twice this size + 1
            // 4. we start in the middle of this space (hence the +1 above)

            var totalLeft = headMovements.Where(m => m.DirectionToMove == 'L').Sum(m => m.StepsToMove);
            var totalRight = headMovements.Where(m => m.DirectionToMove == 'R').Sum(m => m.StepsToMove);
            var currentHeadPosX = (totalLeft >= totalRight) ? totalLeft : totalRight;
            var horizontalSize = currentHeadPosX * 2 + 1;

            var totalUp = headMovements.Where(m => m.DirectionToMove == 'U').Sum(m => m.StepsToMove);
            var totalDown = headMovements.Where(m => m.DirectionToMove == 'D').Sum(m => m.StepsToMove);
            var currentHeadPosY = (totalUp >= totalDown) ? totalUp : totalDown;
            var verticalSize = currentHeadPosY * 2 + 1;

            // the head and tail start in the same position
            headPos.x = currentHeadPosX;
            headPos.y = currentHeadPosY;
            tailPos.x = currentHeadPosX;
            tailPos.y = currentHeadPosY;

            // the starting point is visited
            visitedGridPoints = new bool[horizontalSize, verticalSize];
            visitedGridPoints[headPos.x, headPos.y] = true;
        }

        private static void PartA()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");
        }

        private static void PartB()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");
        }
    }
}