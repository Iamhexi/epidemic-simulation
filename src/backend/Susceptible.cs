using Microsoft.Xna.Framework;
namespace EpidemicSimulation.src.backend
{
    class Susceptible: Person
    {
        public Susceptible(float? immunity = null, int? repulsionRate = null)
            : base(immunity, repulsionRate)
        {

        }

        public override bool IsInfected()
        {
            return false;
        }

        public override string Type()
        {
            return "Susceptible";
        }
    }
}
