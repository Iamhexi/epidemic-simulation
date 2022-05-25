using Microsoft.Xna.Framework;

namespace EpidemicSimulation.src.backend
{
    abstract class Transmission
    {
        public float repulsion;
        public float InfectionProbability;
        public abstract Infecious Infect(Infecious infecious, Susceptible susceptible);
        public abstract Susceptible ToSusceptible();
        public abstract Infecious ToInfecious(Susceptible susceptible);
        public abstract Recovered ToRemovedRecovered(Infecious infecious);
        public abstract Dead ToRemovedDead(Infecious infecious);



    }
}