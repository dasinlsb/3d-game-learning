using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
public class UFOFactory : MonoBehaviour
{
    //private Vector3 hidePosition = new Vector3(0, 0, 0);
    private Vector3 hidePosition = new Vector3(-233, -233, -233);
    Queue<GameObject> ufoPool = new Queue<GameObject>();
    public int ufoKinds = 4;
    public Color[] ufoColors = new Color[]{Color.red, Color.yellow, Color.green, Color.blue};
    public int[] ufoScores = new int[]{1, 2, 3, 4};
    public float[] ufoScales = new float[]{1.5f, 2.0f, 2.5f, 3.0f};
    public void Withdraw(GameObject ufo)
    {
        ufo.transform.position = hidePosition;
        this.ufoPool.Enqueue(ufo);
    }
    public GameObject Launch(int level)
    {
        if (ufoPool.Count == 0) 
        {
            GameObject newufo = Instantiate(Resources.Load<GameObject>("Prefabs/UFO"), 
                                        hidePosition, Quaternion.identity);
            var ufoCtl = newufo.AddComponent<UFOController>() as UFOController;
            ufoPool.Enqueue(newufo);
        }
        GameObject ufo = ufoPool.Dequeue();
        return ufo;
    }
}