using System.Collections.Generic;

namespace EpidemicSimulation
{

    /**
        Interface constituting abstraction of epidemic scenario.
    */

    public interface ISimulation
    {
        void Start();
        void Close();
        void Pause();
        Dictionary<string, int> GetSimulationData();
    }
}
