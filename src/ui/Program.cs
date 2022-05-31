using System;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation.src.backend
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var game = new ShoppingCommunitySimulation(new Point(400,400), 20, 3);
            game.Start();
        }
    }
}