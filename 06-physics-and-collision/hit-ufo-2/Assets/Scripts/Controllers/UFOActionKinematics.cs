using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOActionKinematics : Action
{
    public Vector3 speed;
    public static UFOActionKinematics GetAction(Vector3 speed)
    {
        UFOActionKinematics action = ScriptableObject.CreateInstance<UFOActionKinematics>();
        action.speed = speed;
        return action;
    }

    public override void Update() 
    {
        this.transform.position += Time.deltaTime * this.speed;
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