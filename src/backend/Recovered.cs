using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    class Recovered: Person
    {
        public Recovered(Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(startPosition, MovementVector, immunity, repulsionRate)
        {

        }
        public override bool IsInfected()
        {
            return false;
        }

        public override string Type()
        {
            return "Recovered";
        }

    }
}
