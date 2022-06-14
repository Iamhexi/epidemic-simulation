using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    abstract class Removed: Person
    {
        public Removed(Rectangle simulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(simulationRect, startPosition, MovementVector, immunity, repulsionRate)
        {

        }
    }
}
