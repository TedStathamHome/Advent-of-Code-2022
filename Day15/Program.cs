using System;
using System.IO;
using System.Linq;

namespace Day15
{
    // Puzzle description: https://adventofcode.com/2022/day/15

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 15");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			PartA();
            PartB();
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