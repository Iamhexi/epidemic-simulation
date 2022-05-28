using EpidemicSimulation;
using System.Collections.Generic;

namespace EpidemicSimulation.src.backend
    {
     class MultigroupCommunity : ISimulation
     {
         private List<Simulation> communities = new List<Simulation>();

         public MultigroupCommunity(uint numberOfCommuntiesToSimulate, uint peoplePerSimulation)
         {
             for (int i = 0; i < numberOfCommuntiesToSimulate; i++)
                 GenerateCommunitiy(peoplePerSimulation);
         }

         public void Start()
         {
             foreach (Simulation simulation in communities)
                 simulation.Run();
         }

         private void GenerateCommunitiy(uint sizeOfCommunity)
         {
             communities.Add( new Simulation(sizeOfCommunity) );
         }

     }
    }