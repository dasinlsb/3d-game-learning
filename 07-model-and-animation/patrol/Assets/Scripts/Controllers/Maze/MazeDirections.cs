using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeDirection
{
    Zp,
    Xp,
    Zn,
    Xn,
}

public static class MazeDirectionExtension
{
    public static MazeDirection GetOpposite (this MazeDirection direction)
    {
        return (MazeDirection)(((int)direction + 2) % MazeDirections.Count);
    }
    public static MazeDirection GetNextDirection (this MazeDirection direction)
    {
        return (MazeDirection)(((int)direction + 1) % MazeDirections.Count);
    }
    public static MazeDirection GetPrevDirection (this MazeDirection direction)
    {
        return (MazeDirection)(((int)direction + 3) % MazeDirections.Count);
    }
}




public static class MazeDirections {
    public const int Count = 4;
    private static IntVec2[] steps = {
        new IntVec2(0, 1),
        new IntVec2(1, 0),
        new IntVec2(0, -1),
        new IntVec2(-1, 0),
    };
    private static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f)
	};
    public static MazeDirection RandomDirection {
        get {
            return (MazeDirection)Random.Range(0, Count);
        }
    }
    public static IntVec2 RandomStep {
        get {
            return steps[(int)RandomDirection];
        }
    }
    public static IntVec2 GetStep(MazeDirection index)
    {
        return steps[(int)index];
    }
    public static Quaternion GetRotation (MazeDirection index)
    {
        return rotations[(int)index];
    }
    
}
