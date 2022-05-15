using System;
using System.Collections.Generic;

abstract class Person: IMoveable
{
    public static IAreaScanner areaScanner { set; get; }
    public abstract bool isInfected();
    public static float travelRate {
        get {
            return travelRate;
        }
        set {
            if (inRange(value))
                travelRate = value;
        }
    }

    protected Vector position { get; private set; }
    public float immunity { get; private set; }
    public float repulsionRate { get; private set; }

    public Person(float? immunity = null, float? repulsionRate = null, Vector? intialPosition = null)
    {
        Random random = new Random();
        this.immunity = immunity ?? (float) random.NextDouble();
        this.repulsionRate = repulsionRate ?? (float) random.NextDouble();
    }

    protected Vector calculateRepulsionVector()
    {
        float radius = 5.0f; // beyond this radius all repulsion effects are diminishable
        List<Vector> nearbyPeople = areaScanner.getAllVectorsInRange(position, radius);
        Vector resulantVector = new Vector(0, 0);

        foreach (var person in nearbyPeople)
        {
            float distanceX = position.x - person.x;
            float distanceY = position.y - person.y;
            if (distanceX != 0)
                resulantVector.x = 1 / distanceX;
            if (distanceY != 0)
                resulantVector.y = 1 / distanceY;
        }

        return resulantVector;
    }

    public virtual Vector getResultantVector()
    {
        Vector repulstionVector = calculateRepulsionVector();
        Vector continuousMovement = new Vector(0, 0);
        // TODO:  assign proper the arc-ish random movement vector
        return repulstionVector + continuousMovement;
    }

    protected static bool inRange(float value)
    {
        float min = 0.0f;
        float max = 1.0f;
        return value >= min && value <= max;
    }
}
