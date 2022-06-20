using System;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation.src.backend
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Disease.s_SetUpParams(0.0001f, 2000, 0.021f, 0.3f);

            var game1 = new SingleCommunitySimulation(40, 5); game1.Start();

            var game2 = new ShoppingCommunitySimulation(new Point(500,500),40, 2); game2.Start();

            var game3 = new MultigroupCommunitySimulation(40, 3); game3.Start();
        }
    }
}