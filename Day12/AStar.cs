﻿using System;
using System.Collections.Generic;

namespace Day12
{
    // Implementation of the A* pathfinding algorithm

    public class AStar
    {
        public struct Location
        {
            public readonly short Column, Row;

            public Location(short column, short row)
            {
                Column = column;
                Row = row;
            }
        }

        public interface WeightedGraph<L>
        {
            short ElevationChange(Location startLocation, Location endLocation);
            IEnumerable<Location> Neighbors(Location location);
        }

        public class SquareGrid : WeightedGraph<Location>
        {
            private static readonly Location[] Directions = new[]
            {
                new Location(1, 0),     // one column to the right
                new Location(-1, 0),    // one column to the left
                new Location(0, 1),     // one row down
                new Location(0, -1)     // one row up
            };

            public short Columns { get; set; }
            public short Rows { get; set; }
            public static short[,] Elevation { get; set; }

            public SquareGrid(short columns, short rows, short[,] elevations)
            {
                Columns = columns;
                Rows = rows;
                Elevation = new short[Columns, Rows];

                for (var column = 0; column < Elevation.GetLength(0); column++)
                {
                    for (var row = 0; row < Elevation.GetLength(1); row++)
                    {
                        Elevation[column, row] = elevations[column, row];
                    }
                }
            }

            public short GetElevation(Location location)
            {
                return Elevation[location.Column, location.Row];
            }

            public bool IsInBounds(Location location)
            {
                return 0 <= location.Column && location.Column < Columns
                    && 0 <= location.Row && location.Row < Rows;
            }

            public short ElevationChange(Location startLocation, Location endLocation)
            {
                return (short)(GetElevation(endLocation) - GetElevation(startLocation));
            }

            public IEnumerable<Location> Neighbors(Location location)
            {
                foreach (var direction in Directions)
                {
                    var nextNeighborLocation = new Location((short)(location.Column + direction.Column), (short)(location.Row + direction.Row));

                    if (IsInBounds(nextNeighborLocation))
                        yield return nextNeighborLocation;
                }
            }
        }

        public class PriorityQueue<T>
        {
            private List<Tuple<T, int>> queue = new();

            public int QueueSize => queue.Count;

            public void Enqueue(T queueItem, int priority)
            {
                queue.Add(Tuple.Create(queueItem, priority));
            }

            public T Dequeue()
            {
                int bestItemIndex = 0;

                for (int i = 0; i < queue.Count; i++)
                {
                    if (queue[i].Item2 < queue[bestItemIndex].Item2)
                        bestItemIndex = i;
                }

                var bestItem = queue[bestItemIndex].Item1;
                queue.RemoveAt(bestItemIndex);
                return bestItem;
            }
        }

        public class Search
        {
            public Dictionary<Location, Location> PriorLocation = new();
            public Dictionary<Location, int> TravelCostFromPriorLocation = new();

            public static int Heuristic(Location startLocation, Location endLocation)
            {
                return Math.Abs(startLocation.Column - endLocation.Column) + Math.Abs(startLocation.Row - endLocation.Row);
            }

            public Search(WeightedGraph<Location> graph, Location startLocation, Location finalLocation)
            {
                var frontier = new PriorityQueue<Location>();
                frontier.Enqueue(startLocation, 0);     // no cost for being in the starting location

                PriorLocation[startLocation] = startLocation;
                TravelCostFromPriorLocation[startLocation] = 0;

                while (frontier.QueueSize > 0)
                {
                    var currentLocation = frontier.Dequeue();

                    if (currentLocation.Equals(finalLocation))
                        break;

                    foreach (var nextLocation in graph.Neighbors(currentLocation))
                    {
                        var travelCost = TravelCostFromPriorLocation[currentLocation] + graph.ElevationChange(currentLocation, nextLocation);

                        if (!TravelCostFromPriorLocation.ContainsKey(nextLocation)
                            || travelCost < TravelCostFromPriorLocation[nextLocation])
                        {
                            TravelCostFromPriorLocation[nextLocation] = travelCost;
                            var priority = travelCost + Heuristic(nextLocation, finalLocation);
                            frontier.Enqueue(nextLocation, priority);
                            PriorLocation[nextLocation] = currentLocation;
                        }
                    }
                }
            }
        }
    }
}
