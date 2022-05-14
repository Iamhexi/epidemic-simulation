public struct Vector
{
    public Vector(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public float x { get; }
    public float y { get; }

    public override string ToString() => $"[{x}, {y}]";
}
