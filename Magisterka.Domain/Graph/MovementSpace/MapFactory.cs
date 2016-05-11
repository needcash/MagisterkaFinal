﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magisterka.Domain.Graph.MovementSpace.MapEcosystem;

namespace Magisterka.Domain.Graph.MovementSpace
{
    public class MapFactory
    {
        private const int DefaultNodeNumber = 20;
        private const int DefaultMaxNeighborsForNode = 5;
        private const int MinEdgeCost = 1;
        private const int MaxEdgeCost = 10;
        private const int MinNeighborNumber = 1;
        private readonly Random _randomizer;

        public MapFactory(Random randomizer)
        {
            _randomizer = randomizer;
        }

        public Map GenerateDefaultMap()
        {
            var newMap = new Map(DefaultNodeNumber);

            while (newMap.Count < newMap.MaximumNumberOfNodes)
            {
                newMap.AddIfNotExists(new Node
                {
                    IsBlocked = RandomizeBlockedStatus()
                });
            }

            GenerateNodesNeighbors(ref newMap, DefaultMaxNeighborsForNode);
            PutMapOnGrid(ref newMap);

            return newMap;
        }

        public Map GenerateMapWithProvidedCoordinates(IEnumerable<Position> coordinates)
        {
            var listOfCoordinates = coordinates.ToList();
            var newMap = new Map(listOfCoordinates.Count);

            foreach (var coordinate in listOfCoordinates)
            {
                newMap.AddIfNotExists(new Node
                {
                    IsBlocked = RandomizeBlockedStatus(),
                    Coordinates = coordinate
                });
            }

            GenerateNodesNeighbors(ref newMap, DefaultMaxNeighborsForNode);
            PutMapOnGrid(ref newMap);

            return newMap;
        }

        private void GenerateNodesNeighbors(ref Map map, int maxNumberOfNeighbors)
        {
            foreach (var node in map)
            {
                var numberOfNeighbors = _randomizer.Next(MinNeighborNumber, maxNumberOfNeighbors);
                var newNeighbors =
                    map.Where(
                        otherNode =>
                            otherNode != node && !otherNode.IsNeighborWith(node) &&
                            otherNode.Neighbors.Count < maxNumberOfNeighbors)
                        .Take(numberOfNeighbors - node.Neighbors.Count).Concat(node.Neighbors.Keys).Distinct();

                node.Neighbors = newNeighbors.ToDictionary(x => x, x => new EdgeCost
                {
                    Value = _randomizer.Next(MinEdgeCost, MaxEdgeCost),
                    NodesConnected = new KeyValuePair<Node, Node>(node, x)
                });

                foreach (var neighbor in node.Neighbors.Where(neighbor => !neighbor.Key.IsNeighborWith(node)))
                {
                    neighbor.Key.Neighbors.Add(node, neighbor.Value);
                }
            }
        }

        private void PutMapOnGrid(ref Map map)
        {
            Node firstNode = map.First();
            firstNode.Coordinates.X = 0;
            firstNode.Coordinates.Y = 0;
            GenerateNodesCoordinates(firstNode);
        }

        private void GenerateNodesCoordinates(Node node, int xPosition = 0)
        {
            List<Node> neighbors = node.Neighbors.Keys.Where(x => !x.IsOnTheGrid()).ToList();
            foreach (Node neighbor in neighbors)
            {
                neighbor.Coordinates.Y = node.Coordinates.Y + 1;
                neighbor.Coordinates.X = xPosition;
                ++xPosition;
            }

            xPosition = 0;
            foreach (var neighbor in neighbors)
            {
                GenerateNodesCoordinates(neighbor, xPosition);
                xPosition += neighbor.Neighbors.Count;
            }
        }

        private bool RandomizeBlockedStatus()
        {
            return _randomizer.Next(0, 1) != 0;
        }
    }
}