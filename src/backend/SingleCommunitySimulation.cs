using EpidemicSimulation;

namespace EpidemicSimulation
{
    public class SingleCommunitySimulation : ISimulation
    {
        private Simulation _simulation;

        public SingleCommunitySimulation(uint population)
        {
            _simulation = new Simulation(population);
        }

        public void Start()
        {
            _simulation.Run();
        }
    }
}
