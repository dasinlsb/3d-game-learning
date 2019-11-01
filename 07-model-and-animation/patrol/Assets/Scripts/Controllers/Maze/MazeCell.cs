using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
	public Maze maze;
	public List<Patrol> patrolListeners;
	public Patrol patrol;
	public string role;
    public IntVec2 coord;
	public MazeRoom room;
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
	private int visited_edges = 0;
	public MazeCellEdge GetEdge (MazeDirection direction) {
		return edges[(int)direction];
	}

	public void SetEdge (MazeDirection direction, MazeCellEdge edge) {
		edges[(int)direction] = edge;
		visited_edges++;
	}

	public bool IsFullyVisited 
	{
		get {
			return visited_edges == MazeDirections.Count;
		}
	}
	public MazeDirection RandomNewDirection()
	{
		int remains = MazeDirections.Count - visited_edges;
		int step = Random.Range(0, remains);
		for (int i = 0; i < edges.Length; i++)
		{
			if (edges[i] == null)
			{
				if (step == 0) return (MazeDirection)i;
				step--;
			}
		}
		throw new System.Exception("get RandomNewDirection out of bound !");
	}
	public void InitWithRoom (MazeRoom room)
	{
		room.AddCell(this);
		transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
	}
	public void OnPatrolEntered(string newRole, Patrol newPatrol)
	{
		if (role == "Player")
		{
			maze.gameController.Dead();
			return;
		}
		role = newRole;
		patrol = newPatrol;
	}
	public void OnPatrolExited()
	{
		role = "Empty";
		patrol = null;
	}

	public void OnPlayerEntered(string newRole, MazeDirection direction)
	{
		role = newRole;
		room.Show();
		foreach (Patrol listner in patrolListeners)
		{
			listner.React(coord);
		}
	}
	public void OnPlayerExited(MazeDirection direction)
	{
		role = "Empty";
		room.Hide();
		foreach (Patrol listener in patrolListeners)
		{
			listener.React(new IntVec2(-1, -1));
		}
	}
	public void Hide()
	{
		if (patrol != null)
		{
			patrol.gameObject.SetActive(false);
		}
		gameObject.SetActive(false);
	}
	public void Show()
	{
		if (patrol != null)
		{
			patrol.gameObject.SetActive(true);
		}
		gameObject.SetActive(true);
	}
}