using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    class Infectious: Person
    {
        public new float InfectionDuration = 0;

        public Infectious(float? immunity = null, int? repulsionRate = null) :base(immunity, repulsionRate)
        {

        }

        public Infectious(Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(startPosition, MovementVector, immunity, repulsionRate)
        {

        }

        public void Infect(Susceptible susceptible)
        {

        }

        public override bool IsInfected()
        {
            return true;
        }

        public override string Type()
        {
             return "Infectious";
        }

    }
}
