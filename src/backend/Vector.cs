public struct Vector
{
    public Vector(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public float x { get; set; }
    public float y { get; set; }

    public override string ToString() => $"[{x}, {y}]";

    public static Vector operator+(Vector v, Vector w)
        => new Vector(v.x + w.x, v.y + w.y);

    public static Vector operator*(Vector v, float a)
        => new Vector(v.x*a, v.y*a);
}
