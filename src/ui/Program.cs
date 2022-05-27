using System;

namespace EpidemicSimulation.src.backend
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            ISimulation simulation = new SingleCommunitySimulation(50);
            simulation.Start();
        }
    }
}
