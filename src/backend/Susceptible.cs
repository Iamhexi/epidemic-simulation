using EpidemicSimulation;

namespace EpidemicSimulation
{
    class Susceptible: Person
    {
        public override bool IsInfected()
        {
            return false;
        }
    }
}
