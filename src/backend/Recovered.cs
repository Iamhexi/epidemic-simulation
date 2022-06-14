using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    class Recovered: Person
    {
        public Recovered(Rectangle SimulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(SimulationRect, startPosition, MovementVector, immunity, repulsionRate)
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
