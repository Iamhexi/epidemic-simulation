using EpidemicSimulation;

namespace EpidemicSimulation
{
    public class SingleCommunitySimulation : ISimulation
    {
        private Simulation _simulation;

        public SingleCommunitySimulation(uint population = 20, uint infected = 2)
        {
            _simulation = new Simulation(population, infected);
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
