using UnityEngine;

public class MazeDoor : MazePassage
{
    public Transform hinge;
    public bool opened = false;
    public bool isMirrored;
    private Quaternion normalRotation = Quaternion.Euler(0f, -85f, 0f);
    private Quaternion mirroredRotation = Quaternion.Euler(0f, 85f, 0f);
    private MazeDoor OtherSideDoor
    {
        get {
            return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
        }
    }
    public bool IsOpened {
        get {
            return opened;
        }
    }
    public void SwitchState ()
    {
        if (IsOpened) Close();
        else Open();
    }
    public void Open()
    {
        Debug.Assert(!IsOpened);
        opened = true;
        OtherSideDoor.hinge.localRotation = hinge.localRotation =
            isMirrored ? mirroredRotation : normalRotation;
    }

    public void Close()
    {
        Debug.Assert(IsOpened);
        opened = false;
        OtherSideDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
    }

    public override void BuildBetween (MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        base.BuildBetween (cell, otherCell, direction);
        if (OtherSideDoor != null)
        {
            isMirrored = true;
            hinge.localScale = new Vector3(-1f, 1f, 1f);
            Vector3 oldPos = hinge.localPosition;
            oldPos.x = -oldPos.x;
            hinge.localPosition = oldPos;
        }
        else {
            isMirrored = false;
        }
    }
}