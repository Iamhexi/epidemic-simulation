using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EpidemicSimulation
{
    /**
         This class constitues the shopping community scenario, handling high-level
         events of simulation such as pausing, closing, starting a simulation,
         providing data and providing a simplified constructor.
     */
    class ShoppingCommunitySimulation : Simulation, ISimulation
    {
         /**
            Constructor sets the population and center point.

            @param centerPoint Location of the central point (like shopping mal). It can be null
            @param population Number of people to be simulated
         */
        public ShoppingCommunitySimulation(Point? centerPoint = null, uint population = 20, uint infected = 2):
         base(population, infected)
        {
            if (centerPoint.HasValue) this.CenterPoint = centerPoint;
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
