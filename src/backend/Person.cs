using System;

abstract class Person: IMovement
{
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

    private float immunity { get; }
    private float repulsionRate { get; }

    public Person(float? immunity = null, float? repulsionRate = null, Vector? intialPosition = null)
    {
        Random random = new Random();
        this.immunity = immunity ?? (float) random.NextDouble();
        this.repulsionRate = repulsionRate ?? (float) random.NextDouble();
    }

    public virtual Vector getResultantVector()
    {
        return new Vector(0.0f, 0.0f);
    }

    private static bool inRange(float value)
    {
        float min = 0.0f;
        float max = 1.0f;
        return value >= min && value <= max;
    }
}
