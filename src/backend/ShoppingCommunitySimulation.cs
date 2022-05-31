using System.Collections.Generic;

namespace EpidemicSimulation
 {
     class ShoppingCommunitySimulation : ISimulation
     {
         private Simulation _simulation;

         public ShoppingCommunitySimulation(uint population)
         {
             _simulation = new Simulation(population);
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
