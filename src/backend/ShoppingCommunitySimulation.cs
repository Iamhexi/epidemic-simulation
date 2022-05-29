namespace EpidemicSimulation
 {
     class ShoppingCommunitySimulation : ISimulation
     {
         private Simulation _simulation;

         public ShoppingCommunitySimulation(uint population)
         {
             _simulation = new Simulation(population);
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
