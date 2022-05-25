using System;

namespace EpidemicSimulation.src.backend
{
    public static class Program
    {
        [STAThread]
        static void Main() { using var game = new Simulation(); game.Run(); }
    }
}
