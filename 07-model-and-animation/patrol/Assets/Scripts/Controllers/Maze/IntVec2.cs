using System;

[Serializable]
public struct IntVec2 {
    public int x, z;
    public IntVec2 (int x, int z)
    {
        this.x = x;
        this.z = z;
    }
    public static IntVec2 operator + (IntVec2 a, IntVec2 b) 
    {
        a.x += b.x;
        a.z += b.z;
        return a;
    } 
    public static bool operator != (IntVec2 a, IntVec2 b)
    {
        return a.x != b.x || a.x != b.z;
    }
    public static bool operator == (IntVec2 a, IntVec2 b)
    {
        return a.x == b.x && a.x == b.z;
    }
    public static int ManhattanDistance (IntVec2 a, IntVec2 b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
    }
}
