using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour {

	public MazeCell cell, otherCell;
	
	public MazeDirection direction;

    public virtual void BuildBetween (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		this.cell = cell;
		this.otherCell = otherCell;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
        transform.localRotation = MazeDirections.GetRotation(direction);
	}
	public virtual void OnPlayerEntered()
	{

	}
	public virtual void OnPlayerExited()
	{
		
	}
    private void Start()
    {

    }
    private void Update()
    {

    }
}
