using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation
 {
     /**
         This class constitues the shopping community scenario, handling high-level
         events of simulation such as pausing, closing, starting a simulation,
         providing data and providing a simplified constructor.
     */

     class ShoppingCommunitySimulation : ISimulation
     {
         private Simulation _simulation;

         public ShoppingCommunitySimulation(uint population, Point? centerPoint = null)
         {
            _simulation = new Simulation(population - 1, 1);
            if (centerPoint.HasValue)
                _simulation.CenterPoint = centerPoint;
         }

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
