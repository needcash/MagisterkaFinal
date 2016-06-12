﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magisterka.Domain.Graph.Pathfinding.PathfindingStrategies;
using Magisterka.Domain.Monitoring;

namespace Magisterka.Domain.Graph.Pathfinding
{
    public class PathfinderFactory : IPathfinderFactory
    {
        private readonly AlgorithmMonitor _monitor;

        public PathfinderFactory(AlgorithmMonitor monitor)
        {
            _monitor = monitor;
        }

        public Pathfinder CreatePathfinderWithAlgorithm(ePathfindingAlgorithms algorithm)
        {
            switch (algorithm)
            {
                case ePathfindingAlgorithms.Djikstra:
                    return new Pathfinder(new DijkstraStrategy());
                case ePathfindingAlgorithms.BellmanFord:
                    return new Pathfinder(new BellmanFordStrategy());
                case ePathfindingAlgorithms.AStar:
                    return new Pathfinder(new AStarStrategy());
                case ePathfindingAlgorithms.FloydWarshall:
                    return new Pathfinder(new FloydWarshallStrategy());
                case ePathfindingAlgorithms.Johnson:
                    return new Pathfinder(new JohnsonStrategy());
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
            }
        }
    }
}
