using Microsoft.Xna.Framework;

namespace EpidemicSimulation {
    class Dead: Person
    {
        public Dead(Rectangle SimulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(SimulationRect, startPosition, MovementVector, 0, repulsionRate)
        {

        }

        public override void UpdateSelf()
        {
            base.UpdateSelf();
            this.MovementVector = new Vector2(0,0);
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
