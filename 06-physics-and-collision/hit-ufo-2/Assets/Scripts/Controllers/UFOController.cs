using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UFOController : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public int score;
    public Color color;
    private void Update() 
    {
        // this.transform.position += speed * Time.deltaTime * direction;
    }
}