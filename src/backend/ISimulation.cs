using System.Collections.Generic;

namespace EpidemicSimulation
{
    public interface ISimulation
    {
        void Start();
        void Close();
        void Pause();
        Dictionary<string, int> GetSimulationData();
    }
}
