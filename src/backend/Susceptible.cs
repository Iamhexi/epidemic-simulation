using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    /**
        Class representing a person that is still healthy but prone to get infected.
    */

    class Susceptible: Person
    {
        /**
            Constructor invoked during creation an instance of a concrete class representing a healthy but suscetible person.

            @param simulationRect Rectangle determining an area where this person can move
            @param startPosition Position where the person is located at the very beginning of the simulation or after being added to the simulation
            @param MovementVector Vector defining movement of this person
            @param immunity Number representing personal immunity
            @param repulsionRate Rate at which a person is repulsed from other. It hinders a risk of getting infected.
        */

        public Susceptible(Rectangle simulationRect, float? immunity = null, int? repulsionRate = null)
            : base(simulationRect, immunity, repulsionRate)
        {

        }

        /**
            Constructor invoked during creation an instance of a concrete class representing a healthy but suscetible person.

            @param simulationRect Rectangle determining an area where this person can move
            @param immunity Number representing personal immunity. If it's null, a random value will be assigned
            @param repulsionRate Rate at which a person is repulsed from other. It hinders a risk of getting infected. If it's null, a random value will be assigned
        */

        public Susceptible(Rectangle simulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(simulationRect, startPosition, MovementVector, immunity, repulsionRate)
        {

        }

        /**
            Returns a type of person as a text label.
        */

        public override string Type()
        {
            return "Susceptible";
        }
    }
}
