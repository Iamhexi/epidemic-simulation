using System;
using EpidemicSimulation;
using System.Collections.Generic;

namespace EpidemicSimulation
{
    class MultigroupCommunitySimulation : ISimulation
    {
        private List<Simulation> communities = new List<Simulation>();

        public MultigroupCommunitySimulation(uint numberOfCommuntiesToSimulate, uint peoplePerSimulation)
        {
            for (int i = 0; i < numberOfCommuntiesToSimulate; i++)
                GenerateCommunitiy(peoplePerSimulation);
        }

        public Dictionary<string, int> GetSimulationData()
        {
            Dictionary<string, int> compoundedData = new Dictionary<string, int>();
            compoundedData.Add("Susceptible", 0);
            compoundedData.Add("Infectious", 0);
            compoundedData.Add("Recovered", 0);
            compoundedData.Add("Dead", 0);

            foreach (var c in communities)
            {
                var dictionary = c.GenerateOutputLists();
                compoundedData["Susceptible"] += dictionary["Susceptible"];
                compoundedData["Infectious"] += dictionary["Infectious"];
                compoundedData["Recovered"] += dictionary["Recovered"];
                compoundedData["Dead"] += dictionary["Dead"];
            }

            return compoundedData;
        }

        public void Start()
        {
            foreach (Simulation simulation in communities)
                simulation.Run();
        }

        public void Pause()
        {
            foreach (Simulation simulation in communities)
                simulation.Pause();
        }

        public void Close()
        {
            foreach (Simulation simulation in communities)
                simulation.Exit();
        }

        private void GenerateCommunitiy(uint sizeOfCommunity)
        {
            communities.Add( new Simulation(sizeOfCommunity) );
        }

     }
    }
