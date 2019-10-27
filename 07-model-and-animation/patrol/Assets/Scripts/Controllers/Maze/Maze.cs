using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class Maze : MonoBehaviour {
    
    public IntVec2 size;
	
	public MazeCell cellPrefab;
    public MazePassage passagePrefab;
    public MazeDoor doorPrefab;
    public MazeWall wallPrefab;
    public MazeRoomSettings[] roomSettings;
    [Range(0f, 1f)]
    public float doorProbability;

    private List<MazeRoom> rooms = new List<MazeRoom>();
	private MazeCell[,] cells;
    private float generationStepDelay;

    private static IntVec2[] directions = {
        new IntVec2(0, -1),
        new IntVec2(1, 0),
        new IntVec2(0, 1),
        new IntVec2(-1, 0),
    };

    private void Start() {
    }
    private void Update() {
    }

    public IEnumerator Generate()
    {
        size = new IntVec2(5, 5);
        generationStepDelay = 0.005f;
        cells = new MazeCell[size.x, size.z];
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        List<MazeCell> activeCells = new List<MazeCell>();
        GenFirstCell(activeCells);
        while (activeCells.Count > 0)
        {
            yield return delay;
            ExpandActiveCells(activeCells);
        }
        foreach (var room in rooms)
        {
            room.Hide();
        }
    }
    private void GenFirstCell(List<MazeCell> activeCells)
    {
        MazeCell cell = CreateCell(RandomCoord());
        cell.InitWithRoom(CreateRoom(-1));
        activeCells.Add(cell);
    }
    private void ExpandActiveCells (List<MazeCell> activeCells)
    {
        int index = activeCells.Count - 1;
        MazeCell cell = activeCells[index];
        if (cell.IsFullyVisited)
        {
            activeCells.RemoveAt(index);
            return;
        }
        MazeDirection direction = cell.RandomNewDirection();
        var newCoord = cell.coord + MazeDirections.GetStep(direction);
        if (IsValidCoord(newCoord))
        {
            MazeCell neighbor = GetCell(newCoord);
            if (neighbor == null)
            {
                neighbor = CreateCell(newCoord);
                CreatePassage(cell, neighbor, direction);
                activeCells.Add(neighbor);                
            }
            else if (cell.room.settingsIndex == neighbor.room.settingsIndex)
            {
                CreatePassageInOneRoom(cell, neighbor, direction);
            }
            else
            {
                CreateWall(cell, neighbor, direction);
            }
        } 
        else
        {
            CreateWall(cell, null, direction);
        }
    }
    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage prefab = Random.Range(0.0f, 1.0f) < doorProbability ? doorPrefab : passagePrefab;
        MazePassage passage = Instantiate(prefab) as MazePassage;
		passage.BuildBetween(cell, otherCell, direction);
        if (prefab is MazeDoor)
        {
            otherCell.InitWithRoom(CreateRoom(cell.room.settingsIndex));
        }
        else
        {
            otherCell.InitWithRoom(cell.room);
        }
		passage = Instantiate(prefab) as MazePassage;
		passage.BuildBetween(otherCell, cell, direction.GetOpposite());
    }
    private void CreatePassageInOneRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.BuildBetween(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.BuildBetween(otherCell, cell, direction.GetOpposite());
        if (cell.room != otherCell.room)
        {
            MazeRoom otherRoom = otherCell.room;
            cell.room.Merge(otherRoom);
            rooms.Remove(otherRoom);
            Destroy(otherRoom);
        }
    }
    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.BuildBetween(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MazeWall;
			wall.BuildBetween(otherCell, cell, direction.GetOpposite());
		}
    }
    private MazeRoom CreateRoom (int excludeIndex)
    {
        MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
        newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
        if (newRoom.settingsIndex == excludeIndex)
        {
            newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
        }
        newRoom.settings = roomSettings[newRoom.settingsIndex];
        rooms.Add(newRoom);
        return newRoom;
    }
    private MazeCell CreateCell (IntVec2 coord)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        if (!IsValidCoord(coord))
        {
            Debug.Log("create cell error on coord: " + coord.x + ", " + coord.z);
            Debug.Log("and the size is: " + size.x + ", " + size.z);
        }
        cells[coord.x, coord.z] = newCell;
        newCell.coord = coord;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coord.x - size.x * 0.5f + 0.5f, 0f, coord.z - size.z * 0.5f + 0.5f);
        return newCell;
    }

    public MazeCell GetCell(IntVec2 coord)
    {
        return cells[coord.x, coord.z];
    }

    private bool IsValidCoord(IntVec2 coord)
    {
        return 0 <= coord.x && coord.x < size.x && 0 <= coord.z && coord.z < size.z;
    }

    public IntVec2 RandomCoord()
    {
        return new IntVec2(Random.Range(0, size.x), Random.Range(0, size.z));
    }

}

