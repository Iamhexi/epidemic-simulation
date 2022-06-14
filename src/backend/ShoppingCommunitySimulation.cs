using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
     class ShoppingCommunitySimulation : ISimulation
    {
        private Simulation _simulation;
        public ShoppingCommunitySimulation(Point? centerPoint = null, uint population = 20, uint infected = 2)
        {
            _simulation = new Simulation(population, infected);
            if (centerPoint.HasValue) _simulation.CenterPoint = centerPoint; 
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
