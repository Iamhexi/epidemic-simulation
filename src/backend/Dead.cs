using Microsoft.Xna.Framework;

namespace EpidemicSimulation {
    class Dead: Person
    {
        public Dead(Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(startPosition, MovementVector, 0, repulsionRate)
        {
            
        }
        public override bool IsInfected()
        {
            return false;
        }
        public override string Type()
        {
            return "Dead";
        }
    }
}
