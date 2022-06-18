using EpidemicSimulation;
using System.Collections.Generic;

namespace EpidemicSimulation
{
    /**
        This class constitues the single community scenario, handling high-level
        events of simulation such as pausing, closing, starting a simulation,
        providing data and providing a simplified constructor.
    */

    public class SingleCommunitySimulation : ISimulation
    {
        private Simulation _simulation;

        /**
            Constructor creating an instance of Simulation class taking as a
            parameter the desired population size.

            @param population The desired population size expressed in the number of people.
        */

        public SingleCommunitySimulation(uint population)
        {
            _simulation = new Simulation(population - 3, 3);
        }

        /**
            Returns numbers of infected, susceptible, recovered and dead people.
        */

        public Dictionary<string, int> GetSimulationData()
        {
            return _simulation.GenerateOutputLists();
        }

        /**
            Starts the simulation.
        */

        public void Start()
        {
            _simulation.Run();
        }

        /**
            Pauses and unpauses the simulation.
        */

        public void Pause()
        {
            _simulation.Pause();
        }

        /**
            Closes the simulation.
        */

        public void Close()
        {
            _simulation.Exit();
        }
    }
}
