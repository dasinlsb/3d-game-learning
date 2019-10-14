using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOActionDynamics : Action
{
    public Vector3 speed;
    public static UFOActionDynamics GetAction(Vector3 speed)
    {
        UFOActionDynamics action = ScriptableObject.CreateInstance<UFOActionDynamics>();
        action.speed = speed;
        return action;
    }

    public override void Update() 
    {
        this.transform.position += Time.deltaTime * this.speed;
        this.speed += new Vector3(0, Time.deltaTime * -3.0f, 0);
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
            viewpos.y > 0.0f && viewpos.y < 1.0f;
    }

}