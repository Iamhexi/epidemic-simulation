using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    class Infectious: Person
    {
        public new float InfectionDuration = 0;

        public Infectious(Rectangle simulationRect, float? immunity = null, int? repulsionRate = null) :
            base(simulationRect, immunity, repulsionRate)
        {

        }

        public Infectious(Rectangle simulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(simulationRect, startPosition, MovementVector, immunity, repulsionRate)
        {

        }

        public override string Type()
        {
             return "Infectious";
        }

    }
}
