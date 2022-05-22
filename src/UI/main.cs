using System;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Simulation()) game.Run();
        }
    }
}
