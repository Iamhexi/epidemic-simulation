using System;
using System.Collections.Generic;
using System.IO;

namespace EpidemicSimulation
{
    class StatisticsPrinter
    {
        private const string OUTPUT_FILENAME_PATH = "log/statistics.csv";
        private ISimulation _simulation;

        public StatisticsPrinter(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public void Print()
        {
            StreamWriter writer = new StreamWriter(OUTPUT_FILENAME_PATH, true);
            var data = _simulation.GetSimulationData();

            writer.WriteLine(DateTime.Now.ToString("hh:mm:ss dd-MM-yyyy"));

            writer.WriteLine("Lethality: ~" +  (Disease.Lethality * 1000f) + "%");
            writer.WriteLine("Disease duration: " + ((Disease.Duration - 1500f) / 10f) + " days");
            writer.WriteLine("Communicability: ~" + Disease.Communicability * 16 + "%");

            writer.WriteLine("Population: " + (data["Susceptible"] + data["Infectious"] + data["Recovered"] + data["Dead"]));

            writer.WriteLine("Susceptible: " + data["Susceptible"]);
            writer.WriteLine("Infectious: " + data["Infectious"]);
            writer.WriteLine("Recovered: " + data["Recovered"]);
            writer.WriteLine("Dead: " + data["Dead"]);
            writer.WriteLine();

            writer.Close();
        }
    }
}
