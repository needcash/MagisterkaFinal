﻿using Magisterka.Domain.Monitoring.Behaviours;

namespace Magisterka.Domain.Monitoring
{
    public interface IBehaviourRegistry<out TResults>
        where TResults : IMonitorResults
    {
        void StartRegistration();
        TResults StopRegistration();
        void NoteBehaviour(IAlgorithmBehaviour<TResults> behaviour);
        TResults GetRegisteredResults();
    }
}
