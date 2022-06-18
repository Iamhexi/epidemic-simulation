using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    /**
        Abstract class representing a person that have either died or recovered.
    */

    abstract class Removed: Person
    {

        /**
            Constructor delegating assigning parameters to higher-level abstract constructor (Person).

            @param simulationRect Rectangle determining an area where this person can move
            @param startPosition Position where the person is located at the very beginning of the simulation or after being added to the simulation
            @param MovementVector Vector defining movement of this person
            @param immunity Number representing personal immunity
            @param repulsionRate Rate at which a person is repulsed from other. It hinders a risk of getting infected.
        */

        public Removed(Rectangle simulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(simulationRect, startPosition, MovementVector, immunity, repulsionRate)
        {

        }
    }
}
