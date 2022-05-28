using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    class Infecious: Person
    {
        public Infecious(float? immunity = null, int? repulsionRate = null)
            : base(immunity, repulsionRate)
        {

        }

        public override bool IsInfected()
        {
            return true;
        }

        public override string Type()
        {
             return "Infecious";
        }
    }
}
