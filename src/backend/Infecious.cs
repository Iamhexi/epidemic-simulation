using EpidemicSimulation;

namespace EpidemicSimulation
{
    class Infecious: Person
    {
        public Infecious(float? immunity = null, float? repulsionRate = null)
            : base(immunity, repulsionRate)
        {
            
        }

        public override bool IsInfected()
        {
            return true;
        }
    }
}
