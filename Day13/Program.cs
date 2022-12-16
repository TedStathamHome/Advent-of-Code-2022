using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    // Puzzle description: https://adventofcode.com/2022/day/13

    class PacketPair
    {
        public string Packet1 { get; set; } = string.Empty;
        public string Packet2 { get; set; } = string.Empty;
        public bool PacketsInCorrectOrder { get; set; } = false;
    }

    class Program
    {
        private static List<PacketPair> PacketPairs = new();

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 13");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();

            ParsePuzzleInput(puzzleInputRaw);
            Console.WriteLine($"* Number of packet pairs: {PacketPairs.Count:N0}");

			PartA();
            PartB();
        }

        private static void ParsePuzzleInput(List<string> puzzleInput)
        {
            for (int i = 0; i < puzzleInput.Count; i += 3)
            {
                PacketPairs.Add(new()
                {
                    Packet1 = puzzleInput[i],
                    Packet2 = puzzleInput[i + 1]
                });
            }
        }

        private static void PartA()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");
        }

        private static bool CheckOrderOfPackets(string leftPacket, string rightPacket)
        {
            // we're starting with 2 lists
            if (leftPacket[0] == '[' && rightPacket[0] == '[')
            {
                var leftList = leftPacket[1..^1];
                var rightList = rightPacket[1..^1];

                return CheckOrderOfPackets(leftList, rightList);
            }
            else if (leftPacket[0] == '[' && rightPacket[0] != '[')
            {
                var comma = rightPacket.IndexOf(',');
                if (comma > 0)
                {
                    var newRightPacket = "[" + rightPacket[..comma] + "]" + rightPacket[comma..];
                    return CheckOrderOfPackets(leftPacket, newRightPacket);
                }
                else
                {
                    var newRightPacket = "[" + rightPacket + "]";
                    return CheckOrderOfPackets(leftPacket, newRightPacket);
                }
            }
            else if (rightPacket[0] == '[' && leftPacket[0] != '[')
            {
                var comma = leftPacket.IndexOf(',');
                if (comma > 0)
                {
                    var newLeftPacket = "[" + leftPacket[..comma] + "]" + leftPacket[comma..];
                    return CheckOrderOfPackets(newLeftPacket, rightPacket);
                }
                else
                {
                    var newLeftPacket = "[" + leftPacket + "]";
                    return CheckOrderOfPackets(newLeftPacket, rightPacket);
                }
            }
        }

        private static void PartB()
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");
        }
    }
}