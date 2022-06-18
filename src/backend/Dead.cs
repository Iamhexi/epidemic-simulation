using Microsoft.Xna.Framework;

namespace EpidemicSimulation {

    /**
        Class representing a person that have died due to the simulated epidemic.
    */


    class Dead: Removed
    {
        /**
            Constructor invoked during creation an instance of a concrete class representing a dead person.

            @param simulationRect Rectangle determining an area where this person can move
            @param startPosition Position where the person is located at the very beginning of the simulation or after being added to the simulation
            @param MovementVector Vector defining movement of this person
            @param immunity Number representing personal immunity
            @param repulsionRate Rate at which a person is repulsed from other. It hinders a risk of getting infected.
        */

        public Dead(Rectangle simulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(simulationRect, startPosition, MovementVector, 0, repulsionRate)
        {

        }

        /**
            Returns a type of person as a text label.
        */

        public override string Type()
        {
            return "Dead";
        }
    }
}
