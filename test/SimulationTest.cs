using EpidemicSimulation;

namespace TestSuite
{
    class SimulationTest
    {
        public static void TestGenerateOutputLists()
        {
            uint population = 10;
            uint sum = 0;
            Simulation simulation = new Simulation(population-1, 1);

            var data = simulation.GenerateOutputLists();
            foreach (var num in data)
            sum += (uint) num.Value;

            TestRunner.AssertEquals(population, sum);
        }
    }
}
