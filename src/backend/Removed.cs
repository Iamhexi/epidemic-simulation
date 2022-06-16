using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    abstract class Removed: Person
    {
        public Removed(Rectangle SimulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(SimulationRect, startPosition, MovementVector, immunity, repulsionRate)
        {

        }
    }
}