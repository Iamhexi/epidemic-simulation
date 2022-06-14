using System;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation.src.backend
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            
            var game = new Simulation(20,1); game.Run();

            //var game = new SingleCommunitySimulation(20); game.Start();

            // var game = new ShoppingCommunitySimulation(new Point(300,300),10, 0); game.Start();

            //var game = new MultigroupCommunitySimulation(MultigroupCommunitySimulation.Communities.two, 5, 1); game.Start();
        }
    }
}