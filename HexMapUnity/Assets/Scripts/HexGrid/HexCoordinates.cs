using UnityEngine;

public struct HexCoordinates
{
    [SerializeField] //should make struct fields visible in inspector (HexTile component) => investigate
    private int x, z, y;

    public int X { get { return x; } }
    public int Z { get { return z; } }

    public int Y { get { return y; } }

    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
        this.y = -x - z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    public override string ToString()
    {
        return $"({X}, {Z})";
    }
    public string ToStringOnSeparateLines()
    {
        return $"{X}\n{Z}";
    }
}
