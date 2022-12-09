using System;
using System.IO;
using System.Linq;

namespace Day09
{
    // Puzzle description: https://adventofcode.com/2022/day/9

    class HeadMovement
    {
        public char DirectionToMove { get; set; }
        public int StepsToMove { get; set; }
    }

    class Program
    {
        private static bool[,] visitedGridPoints;

        private class KnotPos
        {
            public int x { get; set; } = 0;
            public int y { get; set; } = 0;
        }

        private class TailPos
        {
            public int x { get; set; } = 0;
            public int y { get; set; } = 0;
        }

        // position trackers for Part A
		private static KnotPos headPos = new KnotPos();
        private static KnotPos tailPos = new KnotPos();

		// position trackers for Part B
		private static List<KnotPos> knotPos = new List<KnotPos>();
		private const int numberOfKnots = 10;

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022: Day 9");
			var puzzleInputRaw = File.ReadLines($"./PuzzleInput-{((args.Length > 0 && args[0].Trim().ToLower() == "test") ? "test" : "full")}.txt").ToList();
            var headMovements = puzzleInputRaw.Select(i => new HeadMovement()
                {
                    DirectionToMove = i[0], 
                    StepsToMove = int.Parse(i[2..]) 
                }).ToList();

            Console.WriteLine($"* Head movements to process: {headMovements.Count:N0}");

            BuildGrid(headMovements);

            Console.WriteLine($"* Grid is {visitedGridPoints.GetLength(0):N0} x {visitedGridPoints.GetLength(1):N0}");
			/*
			Console.WriteLine("* Starting grid:");

			for (int r = 0; r < visitedGridPoints.GetLength(1); r++)
			{
				var line = string.Empty;

				for (int c = 0; c < visitedGridPoints.GetLength(0); c++)
				{
					line += visitedGridPoints[c, r] ? "*" : "-";
				}

				Console.WriteLine($"** {line}");
			}
			*/

			PartA(headMovements);
			PartB(headMovements);
        }

        private static void BuildGrid(List<HeadMovement> headMovements)
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

			for (int k = 0; k < numberOfKnots; k++)
			{
				knotPos.Add(new KnotPos() { x = currentHeadPosX, y = currentHeadPosY });
			}
		}

		private static void PartA(List<HeadMovement> headMovements)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part A");

			foreach (var headMovement in headMovements)
			{
				var xMovement = 0;
				var yMovement = 0;

				//Console.WriteLine($"* Movement: {headMovement.DirectionToMove} / Steps: {headMovement.StepsToMove:N0}");

				if ("LR".IndexOf(headMovement.DirectionToMove) != -1)
				{
					xMovement = headMovement.DirectionToMove == 'R' ? 1 : -1;
				}
				else
				{
					yMovement = headMovement.DirectionToMove == 'D' ? 1 : -1;
				}

				//Console.WriteLine($"* Head is moving {xMovement:N0}:{yMovement:N0}");
				//Console.WriteLine($"* Head starts at {headPos.x:N0}:{headPos.y:N0}");

				for (int s = 0; s < headMovement.StepsToMove; s++)
				{
					// move the head
					headPos.x += xMovement;
					headPos.y += yMovement;

					//Console.WriteLine($"* Head moved to {headPos.x:N0}:{headPos.y:N0}");

					// move the tail towards the head

					// if the tail is within 1 grid position of the head in any direction,
					// it doesn't need to move
					if (int.Abs(headPos.x - tailPos.x) <= 1 && int.Abs(headPos.y - tailPos.y) <= 1)
					{
						//Console.WriteLine($"* Step {s:N0}: Within 1 grid position; tail doesn't move");
						continue;
					}

					// if the tail is not on the same row and column as the head,
					// then the tail needs to move diagonally in the direction of the head
					if (headPos.x != tailPos.x && headPos.y != tailPos.y)
					{

						// the tail shouldn't need to move more than 1 grid
						// position in any direction
						var tailMoveX = headPos.x - tailPos.x;
						if (int.Abs(tailMoveX) > 1)
							tailMoveX -= tailMoveX > 0 ? 1 : -1;

						var tailMoveY = headPos.y - tailPos.y;
						if (int.Abs(tailMoveY) > 1)
							tailMoveY -= tailMoveY > 0 ? 1 : -1;

						tailPos.x += tailMoveX;
						tailPos.y += tailMoveY;

						//Console.WriteLine($"* Step {s:N0}: Tail moved diagonally to {tailPos.x:N0}:{tailPos.y:N0}");
					}
					else if (headPos.x != tailPos.x)
					{
						// if the head moved left/right of the tail on the same row,
						// then the tail needs to move 1 grid position in a straight line to the head
						var tailMoveX = headPos.x - tailPos.x;
						if (int.Abs(tailMoveX) > 1)
							tailMoveX -= tailMoveX > 0 ? 1 : -1;

						tailPos.x += tailMoveX;

						//Console.WriteLine($"* Step {s:N0}: Tail moved horizontally to {tailPos.x:N0}:{tailPos.y:N0}");
					}
					else
					{
						// the head must have moved up/down from the tail on the same column
						var tailMoveY = headPos.y - tailPos.y;
						if (int.Abs(tailMoveY) > 1)
							tailMoveY -= tailMoveY > 0 ? 1 : -1;

						tailPos.y += tailMoveY;
						//Console.WriteLine($"* Step {s:N0}: Tail moved vertically to {tailPos.x:N0}:{tailPos.y:N0}");
					}

					visitedGridPoints[tailPos.x, tailPos.y] = true;
				}
			}
			
			//Console.WriteLine("\r\n* Grid points visited by tail:");

			var gridPointsVisitedByTail = 0;

			for (int r = 0; r < visitedGridPoints.GetLength(1); r++)
			{
				//var line = string.Empty;

				for (int c = 0; c < visitedGridPoints.GetLength(0); c++)
				{
					gridPointsVisitedByTail += visitedGridPoints[c, r] ? 1 : 0;
					//line += visitedGridPoints[c, r] ? "*" : "-";
				}

				//Console.WriteLine($"** {line}");
			}

			Console.WriteLine($"*** Points visited by the tail: {gridPointsVisitedByTail:N0}");
		}

		private static void PartB(List<HeadMovement> headMovements)
        {
            Console.WriteLine("\r\n**********");
            Console.WriteLine("* Part B");
		
			// need to rebuild the grid prior to the second part of the puzzle
			BuildGrid(headMovements);

			foreach (var headMovement in headMovements)
			{
				var xMovement = 0;
				var yMovement = 0;

				if ("LR".IndexOf(headMovement.DirectionToMove) != -1)
				{
					xMovement = headMovement.DirectionToMove == 'R' ? 1 : -1;
				}
				else
				{
					yMovement = headMovement.DirectionToMove == 'D' ? 1 : -1;
				}

				for (int s = 0; s < headMovement.StepsToMove; s++)
				{
					// move the head, which is at knot index 0
					knotPos[0].x += xMovement;
					knotPos[0].y += yMovement;

					// knot at index 0 is the head
					// knot at last index is the tail
					for (int k = 0; k < knotPos.Count - 1; k++)
					{
						// treat the knot at index k as the current head
						// treat the knot at index k + 1 as the current tail

						// move the current tail towards the current head

						// if the tail is within 1 grid position of the head in any direction,
						// it doesn't need to move
						if (int.Abs(knotPos[k].x - knotPos[k + 1].x) <= 1 && int.Abs(knotPos[k].y - knotPos[k + 1].y) <= 1)
						{
							continue;
						}

						// if the tail is not on the same row and column as the head,
						// then the tail needs to move diagonally in the direction of the head
						if (knotPos[k].x != knotPos[k + 1].x && knotPos[k].y != knotPos[k + 1].y)
						{
							// the tail shouldn't need to move more than 1 grid
							// position in any direction
							var tailMoveX = knotPos[k].x - knotPos[k + 1].x;
							if (int.Abs(tailMoveX) > 1)
								tailMoveX -= tailMoveX > 0 ? 1 : -1;

							var tailMoveY = knotPos[k].y - knotPos[k + 1].y;
							if (int.Abs(tailMoveY) > 1)
								tailMoveY -= tailMoveY > 0 ? 1 : -1;

							knotPos[k + 1].x += tailMoveX;
							knotPos[k + 1].y += tailMoveY;
						}
						else if (knotPos[k].x != knotPos[k + 1].x)
						{
							// if the head moved left/right of the tail on the same row,
							// then the tail needs to move 1 grid position in a straight line to the head
							var tailMoveX = knotPos[k].x - knotPos[k + 1].x;
							if (int.Abs(tailMoveX) > 1)
								tailMoveX -= tailMoveX > 0 ? 1 : -1;

							knotPos[k + 1].x += tailMoveX;
						}
						else
						{
							// the head must have moved up/down from the tail on the same column
							var tailMoveY = knotPos[k].y - knotPos[k + 1].y;
							if (int.Abs(tailMoveY) > 1)
								tailMoveY -= tailMoveY > 0 ? 1 : -1;

							knotPos[k + 1].y += tailMoveY;
						}
					}

					//Console.WriteLine($"** Tail at {knotPos[numberOfKnots - 1].x:N0}:{knotPos[numberOfKnots - 1].y:N0}");
					visitedGridPoints[knotPos[numberOfKnots - 1].x, knotPos[numberOfKnots - 1].y] = true;
				}
			}

			//Console.WriteLine("\r\n* Grid points visited by tail:");
			
			var gridPointsVisitedByTail = 0;

			for (int r = 0; r < visitedGridPoints.GetLength(1); r++)
			{
				//var line = string.Empty;

				for (int c = 0; c < visitedGridPoints.GetLength(0); c++)
				{
					gridPointsVisitedByTail += visitedGridPoints[c, r] ? 1 : 0;
					//line += visitedGridPoints[c, r] ? "*" : "-";
				}

				//Console.WriteLine($"** {line}");
			}

			Console.WriteLine($"*** Points visited by the tail: {gridPointsVisitedByTail:N0}");
		}
	}
}