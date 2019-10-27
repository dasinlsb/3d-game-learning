using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRoom : ScriptableObject
{
    public int settingsIndex;
    public MazeRoomSettings settings;
    private List<MazeCell> cells = new List<MazeCell>();
    private bool visible = true;
    public void AddCell (MazeCell cell)
    {
        cell.room = this;
        cells.Add(cell);
    }
    public void Merge (MazeRoom otherRoom)
    {
        foreach (MazeCell cell in otherRoom.cells)
        {
            AddCell(cell);
        }
    }
    public void Hide()
    {
        if (!visible) return;
        visible = false;
        foreach (var cell in cells)
        {
            cell.Hide();
        }
    }
    public void Show()
    {
        if (visible) return;
        visible = true;
        foreach (var cell in cells)
        {
            cell.Show();
        }
    }
}
