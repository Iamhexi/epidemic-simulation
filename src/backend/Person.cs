using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

abstract class Person
{
    public abstract bool IsInfected();
    public static float TravelRate {
        get;
        set {
            if (inRange(value))
                TravelRate = value;
        }
    }

    public Vector2 Position { get; private set; }
    public Vector2 Size { get; private set; }

    public float ImmunityRate { get; private set; }
    public float RepulsionRate { get; private set; }

    public Person(float? immunity = null, float? repulsionRate = null)
    {
        Random random = new Random();
        this.ImmunityRate = immunity ?? (float) random.NextDouble();
        this.RepulsionRate = repulsionRate ?? (float) random.NextDouble();
    }

    protected static bool inRange(float value)
    {
        float min = 0.0f;
        float max = 1.0f;
        return value >= min && value <= max;
    }
}
