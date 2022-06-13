using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    class Recovered: Removed
    {
        public Recovered(Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(startPosition, MovementVector, 0, repulsionRate)
        {

        }

        public override string Type()
        {
            return "Recovered";
        }

    }
}
