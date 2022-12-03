using System;
using System.IO;
using System.Linq;

namespace Day03
{
    // Puzzle description: https://adventofcode.com/2022/day/3

    class Program
    {
        private const string itemPriorities = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 3");
			var rucksackContents = File.ReadLines($"./RucksackContents-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

			PartA(rucksackContents);
            PartB(rucksackContents);
        }

        private static void PartA(List<string> rucksackContents)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

            var commonItems = string.Empty;
            var sumOfPriorities = 0;

            foreach (string rucksack in rucksackContents)
            {
                var compartmentItems = rucksack.Length / 2;
                var leftCompartment = rucksack.Substring(0, compartmentItems);
                var rightCompartment = rucksack.Substring(compartmentItems);
                var foundCommonItems = leftCompartment.Intersect(rightCompartment).ToList();

                Console.WriteLine($"\r\n* Left:   {leftCompartment}");
                Console.WriteLine($"* Right:  {rightCompartment}");
                Console.WriteLine($"* Common: {string.Join("", foundCommonItems)}");
                
                commonItems += string.Join("", foundCommonItems);
                sumOfPriorities += ItemPriority(foundCommonItems[0]);
            }

            Console.WriteLine($"*** All common items: {commonItems}");
            Console.WriteLine($"*** Sum of priorities: {sumOfPriorities:N0}");
        }

        private static void PartB(List<string> rucksackContents)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");

            var groupBadges = string.Empty;

            for (int i = 0; i < rucksackContents.Count; i += 3)
            {
                var rucksack1 = rucksackContents[i];
                var rucksack2 = rucksackContents[i + 1];
                var rucksack3 = rucksackContents[i + 2];

                Console.WriteLine($"\r\n* RS#1 {rucksack1}");
                Console.WriteLine($"* RS#2 {rucksack2}");
                Console.WriteLine($"* RS#3 {rucksack3}");

                var teamBadge = string.Join("", rucksack1.Intersect(rucksack2.Intersect(rucksack3)));
                Console.WriteLine($"* Team badge: {teamBadge}");
                groupBadges += teamBadge;
            }

            var sumOfPriorities = groupBadges.Sum(b => ItemPriority(b));

            Console.WriteLine($"*** Sum of priorities: {sumOfPriorities:N0}");
        }

        private static int ItemPriority(char itemCode)
        {
            return itemPriorities.IndexOf(itemCode);
        }
    }
}