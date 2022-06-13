using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    abstract class Removed: Person
    {
        public Removed(Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(startPosition, MovementVector, immunity, repulsionRate)
        {

        }
    }
}
