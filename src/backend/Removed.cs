namespace EpidemicSimulation
{
    abstract class Removed: Person
    {
        public override bool IsInfected()
        {
            return false;
        }
    }
}
