namespace EpidemicSimulation.src.backend
{
    class Recovered: Removed
    {

        public override bool IsInfected()
        {
            return false;
        }
        public override string Type() { return "Recovered"; }
    }
}
