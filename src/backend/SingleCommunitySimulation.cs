using EpidemicSimulation;
using System.Collections.Generic;

namespace EpidemicSimulation
{
    public class SingleCommunitySimulation : ISimulation
    {
        private Simulation _simulation;

        public SingleCommunitySimulation(uint population)
        {
            _simulation = new Simulation(population - 3, 3);
        }

        public Dictionary<string, int> GetSimulationData()
        {
            return _simulation.GenerateOutputLists();
        }

        public void Start()
        {
            _simulation.Run();
        }

        public void Pause()
        {
            _simulation.Pause();
        }

        public void Close()
        {
            _simulation.Exit();
        }
    }
}
