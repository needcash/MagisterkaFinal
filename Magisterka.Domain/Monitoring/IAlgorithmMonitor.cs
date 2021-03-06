﻿using System.ComponentModel;
using Magisterka.Domain.Graph.MovementSpace.MapEcosystem;
using Magisterka.Domain.Monitoring.Performance;
using Magisterka.Domain.Monitoring.Quality;

namespace Magisterka.Domain.Monitoring
{
    public interface IAlgorithmMonitor : INotifyPropertyChanged
    {
        PathDetails PathDetails { get; set; }
        PerformanceResults PerformanceResults { get; set; }
        bool IsMonitoring { get; set; }

        void StartMonitoring();
        void StopMonitoring();
        void RecordStep();
        void RecordNodeProcessed(params Node[] processedNodes);
        void RecordEdgeCost(Node fromNode, Node toNode);
        void MonitorPathFragment(Node fromNode, Node toNode);
        void Clear();
    }
}
