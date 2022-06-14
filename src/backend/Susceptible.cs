using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    class Susceptible: Person
    {
        public Susceptible(Rectangle simulationRect, float? immunity = null, int? repulsionRate = null)
            : base(simulationRect, immunity, repulsionRate)
        {

        }
        public Susceptible(Rectangle simulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(simulationRect, startPosition, MovementVector, immunity, repulsionRate)
        {

        }

        public override string Type()
        {
            return "Susceptible";
        }
    }
}
