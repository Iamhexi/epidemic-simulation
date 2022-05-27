namespace EpidemicSimulation.src.backend
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
    }
}
