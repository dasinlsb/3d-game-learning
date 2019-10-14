using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOAction : Action
{
    public float speed;
    public Vector3 direction;
    public static UFOAction GetAction(float speed, Vector3 direction)
    {
        UFOAction action = ScriptableObject.CreateInstance<UFOAction>();
        action.speed = speed;
        action.direction = direction;
        return action;
    }

    public override void Update() 
    {
        this.transform.position += this.speed * Time.deltaTime * this.direction;
        if (!isInView(this.transform.position))
        {
            this.destroy = true;
            this.enable = false;
            this.callback.ActionEvent(this);
        }
    }

    public override void Start() 
    {

    }

    private bool isInView(Vector3 position) 
    {
        Vector3 viewpos = Camera.main.WorldToViewportPoint(position);
        return viewpos.x > 0.0f && viewpos.x < 1.0f &&
            viewpos.y > 0.0 && viewpos.y < 1.0f;
    }

}