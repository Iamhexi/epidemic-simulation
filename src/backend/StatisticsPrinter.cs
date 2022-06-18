using System;
using System.Collections.Generic;
using System.IO;

namespace EpidemicSimulation
{

    /**
        Class saves statistics to an external text file.
    */

    class StatisticsPrinter
    {
        private const string OUTPUT_FILENAME_PATH = "log/statistics.txt";
        private ISimulation _simulation;

        /**
            Constructor takes an instance of ISimulation and assigns its reference
            to class's private property.

            @param simulation An instance of ISimulation where the data will be culled from.
        */

        public StatisticsPrinter(ISimulation simulation)
        {
            _simulation = simulation;
        }

        /**
            Creates a new file if none exists, otherwise, doesn't create or override
            anything. Saves information including date, time, chosen lethality,
            disease duration, communicability, overall population and the final
            number of infected, uninfected, recovered and dead people to the file.
            Closes that file.
        */

        public void Print()
        {
            FileStream fileStream = new FileStream(OUTPUT_FILENAME_PATH, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fileStream);
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
