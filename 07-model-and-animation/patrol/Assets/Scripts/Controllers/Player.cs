using UnityEngine;

public class Player : MonoBehaviour
{
    public MazeCell currentCell;
    public MazeDirection currentDirection;

    public void EnterCell (MazeCell newCell, MazeDirection direction)
    {
        currentCell.OnPlayerExited(direction);
        currentCell = newCell;
        transform.localPosition = newCell.transform.localPosition;
        currentCell.OnPlayerEntered(direction.GetOpposite());
    }
    public void SetLocation (MazeCell newCell)
    {
        newCell.room.Show();
        currentCell = newCell;
        transform.localPosition = newCell.transform.localPosition;
    }
    private void TryMove (MazeDirection direction)
    {
        MazeCellEdge edge = currentCell.GetEdge(direction);
        if (edge is MazePassage)
        {
            if (edge is MazeDoor && !(edge as MazeDoor).IsOpened)
            {
                return;
            }
            EnterCell(edge.otherCell, direction);
        }
    }
    
    private void Look (MazeDirection direction)
    {
        transform.localRotation = MazeDirections.GetRotation(direction);
        currentDirection = direction;
    }

    private void TryOperateDoor (MazeDirection direction)
    {
        MazeCellEdge edge = currentCell.GetEdge(direction);
        if (!(edge is MazeDoor)) return;
        var door = edge as MazeDoor;
        door.SwitchState();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("up key");
            TryMove(currentDirection);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMove(currentDirection.GetNextDirection());
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryMove(currentDirection.GetOpposite());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMove(currentDirection.GetPrevDirection());
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Look(currentDirection.GetPrevDirection());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Look(currentDirection.GetNextDirection());
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            TryOperateDoor(currentDirection);
        }
    }
}