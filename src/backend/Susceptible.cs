using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    class Susceptible: Person
    {
        public Susceptible(float? immunity = null, int? repulsionRate = null)
            : base(immunity, repulsionRate)
        {

        }
        public Susceptible(Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(startPosition, MovementVector, immunity, repulsionRate)
        {

        }

        public override string Type()
        {
            return "Susceptible";
        }
    }
}
