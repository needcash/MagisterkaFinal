﻿using System.Collections.Generic;
using System.Linq;
using Magisterka.Domain.AppSettings;
using Magisterka.Domain.Graph.MovementSpace.MapEcosystem;
using Magisterka.Domain.Utilities;

namespace Magisterka.Domain.Graph.MovementSpace
{
    public class MapFactory : IMapFactory
    {
        private readonly int DefaultNodeNumber;
        private readonly int DefaultMaxNeighborsForNode;
        private readonly int MinEdgeCost;
        private readonly int MaxEdgeCost;
        private readonly int MinNeighborNumber;

        private const string NodeNamePrefix = "Node";

        private readonly IRandomGenerator _randomizer;

        public MapFactory(IRandomGenerator randomizer, IAppSettings appSettings)
        {
            _randomizer = randomizer;
            DefaultNodeNumber = appSettings.RandomGraphDefaultNodeNumber;
            DefaultMaxNeighborsForNode = appSettings.RandomGraphDefaultMaxNeighborsForNode;
            MinEdgeCost = appSettings.RandomGraphMinEdgeCost;
            MaxEdgeCost = appSettings.RandomGraphMaxEdgeCost;
            MinNeighborNumber = appSettings.RandomGraphMinNeighborNumber;
        }

        public Map GenerateDefaultMap()
        {
            return GenerateMap(DefaultNodeNumber, DefaultMaxNeighborsForNode);
        }

        public Map GenerateMapWithProvidedCoordinates(IEnumerable<Position> coordinates)
        {
            var listOfCoordinates = coordinates.ToList();
            var newMap = new Map(listOfCoordinates.Count);
            var nodeCounter = 0;

            foreach (var coordinate in listOfCoordinates)
            {
                var node = GenerateNewNode(nodeCounter);
                node.Coordinates = coordinate;
                ++nodeCounter;
                newMap.AddIfNotExists(node);
            }

            GenerateNodesNeighbors(ref newMap, DefaultMaxNeighborsForNode);

            return newMap;
        }

        public Map GenerateMap(int numberOfNodes, int maxNumberOfNeighborsPerNode)
        {
            var newMap = new Map(numberOfNodes);
            var nodeCounter = 0;

            while (newMap.Count < newMap.MaximumNumberOfNodes)
            {
                newMap.AddIfNotExists(GenerateNewNode(nodeCounter));
                ++nodeCounter;
            }

            GenerateNodesNeighbors(ref newMap, maxNumberOfNeighborsPerNode);

            return newMap;
        }

        public Node GenerateNewNode(int nodesCount)
        {
            return new Node($"{NodeNamePrefix} {nodesCount + 1}");
        }

        private void GenerateNodesNeighbors(ref Map map, int maxNumberOfNeighbors)
        {
            foreach (var node in map)
            {
                var numberOfNeighbors = _randomizer.GenerateNumberOfNeighbors(MinNeighborNumber, maxNumberOfNeighbors);
                var newNeighbors =
                    map.Where(
                        otherNode =>
                            otherNode != node && !otherNode.IsNeighborWith(node) &&
                            otherNode.Neighbors.Count < maxNumberOfNeighbors)
                        .Take(numberOfNeighbors - node.Neighbors.Count).Concat(node.Neighbors.Keys).Distinct();

                node.Neighbors = newNeighbors.ToDictionary(x => x, x => new Edge
                {
                    Cost = _randomizer.GenerateEdgeCost(MinEdgeCost, MaxEdgeCost),
                    NodesConnected = new KeyValuePair<Node, Node>(node, x)
                });

                foreach (var neighbor in node.Neighbors.Where(neighbor => !neighbor.Key.IsNeighborWith(node)))
                {
                    neighbor.Key.Neighbors.Add(node, neighbor.Value);
                }
            }
        }
    }
}