using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    class Infecious: Person
    {
        public new float InfectionDuration = 0;

        public Infecious(Rectangle SimulationRect, float? immunity = null, int? repulsionRate = null) :base(SimulationRect, immunity, repulsionRate)
        {

        }

        public Infecious(Rectangle SimulationRect,Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(SimulationRect, startPosition, MovementVector, immunity, repulsionRate)
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
