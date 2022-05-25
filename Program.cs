using System;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation.src.backend
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {

            using var game = new Simulation(); game.Run();



            /*            System.Diagnostics.Debug.WriteLine("---");
                        Vector2 mowy = new Vector2(1,5);
                        mowy.Normalize();
                        System.Diagnostics.Debug.WriteLine(mowy.ToString());

                        mowy.X += 10;
                        mowy.Normalize();
                        System.Diagnostics.Debug.WriteLine(mowy.ToString());*/

        }
    }
}
