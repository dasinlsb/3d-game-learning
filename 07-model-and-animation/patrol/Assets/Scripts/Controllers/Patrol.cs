using UnityEngine;

public class Patrol : MonoBehaviour 
{
    public int patrolDistance;
    //public int attackDistance;
    public MazeCell currentCell;
    public MazeDirection currentDirection;
    public IntVec2 centerCoord;
    public Maze maze;
    public IntVec2 targetCoord;

    public void React (IntVec2 playerCoord)
    {
        targetCoord = playerCoord;
    }

    private void Start ()
    {

    }

    public void EnterCell (MazeCell newCell)
    {
        currentCell.OnPatrolExited();
        SetLocation(newCell);
    }

    private void Update ()
    {
        if (Random.Range(0, 100) > 95)
        {
            for (int tryTimes = 8; tryTimes > 0; tryTimes--)
            {
                IntVec2 step = MazeDirections.RandomStep;
                IntVec2 newCoord = currentCell.coord + step;
                if (IsValidCoord(newCoord))
                {
                    if (targetCoord != new IntVec2(-1, -1) && 
                        IntVec2.ManhattanDistance(newCoord, targetCoord) > IntVec2.ManhattanDistance(currentCell.coord, targetCoord))
                    {
                        continue;
                    }
                    MazeCell newCell = maze.GetCell(newCoord);
                    if (newCell.room == currentCell.room && newCell.role != "Patrol")
                    {
                        EnterCell(newCell);
                    }
                }
            }
        }
    }

    private bool IsValidCoord (IntVec2 coord)
    {
        return maze.IsValidCoord(coord) && IntVec2.ManhattanDistance(coord, centerCoord) < patrolDistance;
    }

    public void UpdateSubscribe ()
    {
        for (int i = -patrolDistance; i <= patrolDistance; i++)
        {
            for (int j = -patrolDistance; j <= patrolDistance; j++)
            {
                IntVec2 coord = centerCoord + new IntVec2(i, j);
                if (IsValidCoord(coord))
                {
                    MazeCell cell = maze.GetCell(coord);
                    cell.patrolListeners.Add(this);
                }
            }
        }
    }

    public void SetLocation (MazeCell newCell)
    {
        newCell.OnPatrolEntered("Patrol", this);
        currentCell = newCell;
        transform.localPosition = newCell.transform.localPosition;
    }
}