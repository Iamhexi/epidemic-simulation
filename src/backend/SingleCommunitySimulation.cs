using EpidemicSimulation;
using System.Collections.Generic;

namespace EpidemicSimulation
{
     /**
        This class constitues the single community scenario, handling high-level
        events of simulation such as pausing, closing, starting a simulation,
        providing data and providing a simplified constructor.
    */
    class SingleCommunitySimulation : Simulation, ISimulation
    {
        /**
            Constructor creating an instance of Simulation class taking as a
            parameter the desired population size.

            @param population The desired population size expressed in the number of people.
        */
        public SingleCommunitySimulation(uint population = 20, uint infected = 2):
            base(population, infected)
        {

        }
         /**
            Starts the simulation.
        */
        public void Start()
        {
            Run();
        }
        /**
            Closes the simulation.
        */
        public void Close()
        {
            Exit();
        }

        /**
            Returns numbers of dead, infected, healthy and recovered people.
        */

        public Dictionary<string, int> GetSimulationData()
        {
            return GenerateOutputLists();
        }
    }
}
