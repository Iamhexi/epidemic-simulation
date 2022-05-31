using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    class Infecious: Person
    {
        public new float InfectionDuration = 0;

        public Infecious(float? immunity = null, int? repulsionRate = null) :base(immunity, repulsionRate)
        {

        }

        public Infecious(Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
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
             return "Infecious";
        }

    }
}
