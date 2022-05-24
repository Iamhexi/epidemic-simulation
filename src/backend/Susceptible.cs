using EpidemicSimulation;

namespace EpidemicSimulation
{
    abstract class Susceptible: Person
    {
        public Susceptible(float? immunity = null, float? repulsionRate = null)
            : base(immunity, repulsionRate)
        {
            
        }

        public override bool IsInfected()
        {
            return false;
        }
    }
}
