using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
     class ShoppingCommunitySimulation : Simulation, ISimulation
    {
        public ShoppingCommunitySimulation(Point? centerPoint = null, uint population = 20, uint infected = 2): base(population, infected)
        {
            if (centerPoint.HasValue) this.CenterPoint = centerPoint; 
        }

        public void Start()
        {
            Run();
        }

        public void Close()
        {
            Exit();
        }
    }
}
