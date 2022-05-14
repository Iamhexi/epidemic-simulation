class Susceptible: Person
{
    public Susceptible(float? immunity = null, float? repulsionRate = null, Vector? intialPosition = null):
        base(immunity, repulsionRate, intialPosition)
    {

    }

    public override bool isInfected()
    {
        return false;
    }
}
