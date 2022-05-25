namespace EpidemicSimulation.src.backend
{
    class Dead: Removed
    {
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
