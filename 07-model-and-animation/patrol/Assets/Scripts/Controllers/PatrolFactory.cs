using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolFactory : MonoBehaviour {
    public Patrol patrolPrefab;
    private void Start () 
    {

    }

    private void Update ()
    {

    }
    public Patrol Generate(Maze maze)
    {
        Patrol patrol = Instantiate(patrolPrefab) as Patrol;
        patrol.maze = maze;
        patrol.patrolDistance = 3;
        patrol.targetCoord = new IntVec2(-1, -1);
        patrol.UpdateSubscribe();
        return patrol;
    }
}