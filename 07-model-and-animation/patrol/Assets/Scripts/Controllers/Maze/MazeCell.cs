using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
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
	public void OnPlayerEntered(MazeDirection direction)
	{
		room.Show();
	}
	public void OnPlayerExited(MazeDirection direction)
	{
		room.Hide();
	}
	public void Hide()
	{
		gameObject.SetActive(false);
	}
	public void Show()
	{
		gameObject.SetActive(true);
	}
}