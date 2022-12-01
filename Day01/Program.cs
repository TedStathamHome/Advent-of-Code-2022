using System;
using System.IO;
using System.Linq;

namespace Day01
{
    // Puzzle description: https://adventofcode.com/2022/day/1

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 1");
            var elfCaloriesRaw = File.ReadLines(@"\.ElfCalories-test.txt").ToList();
            //var elfCaloriesRaw = File.ReadLines(@"\.ElfCalories-full.txt").ToList();

            var elvesInParty = elfCaloriesRaw.Where(e => string.IsNullOrEmpty(e)).ToList().Count + 1

            Console.WriteLine($"* Elf calorie entries read: {elfCaloriesRaw.Count:N0}");
            Console.WriteLine($"* Elves in party: {elvesInParty:N0}");

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