﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magisterka.Domain.Monitoring
{
    public class AlgorithmMonitor
    {
        public PathDetails PathDetails { get; set; }
        public PerformanceResults PerformanceResults { get; set; }
        public bool IsMonitoring { get; set; }

        public AlgorithmMonitor()
        {
            PathDetails = new PathDetails();
            PerformanceResults = new PerformanceResults();
        }

        public void StartMonitoring()
        {
            IsMonitoring = true;
        }

        public void StopMonitoring()
        {
            IsMonitoring = false;
        }
    }
}
