namespace EpidemicSimulation.src.backend
{
    public struct Disease
    {
        public float Lethality;
        public float Duration;// FIXME: What range can this parameter take?
        public float InfectionProbabilty;
        public int InfectionRadius;
    }
}
