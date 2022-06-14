using Microsoft.Xna.Framework;
namespace EpidemicSimulation
{
    class Susceptible: Person
    {
        public Susceptible(Rectangle SimulationRect, float? immunity = null, int? repulsionRate = null)
            : base(SimulationRect, immunity, repulsionRate)
        {

        }
        public Susceptible(Rectangle SimulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(SimulationRect, startPosition, MovementVector, immunity, repulsionRate)
        {

        }
        public override bool IsInfected()
        {
            return false;
        }
        public override string Type()
        {
            return "Susceptible";
        }
    }
}
