using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
	private IUserAction action;
	private const int buttonX = 150;
	private const int buttonY = 50;
	private const int gridX = 160;
	private const int gridY = 80;

    // Start is called before the first frame update
    void Start()
    {
        action = Director.getInstance().currentSceneController as IUserAction;
    }

    // Update is called once per frame
    void OnGUI() 
	{
		if (GUI.Button(new Rect(0, 0, buttonX, buttonY), "START"))
		{
			action.StartGame();
		}
    	if (GUI.Button(new Rect(buttonX + 30, 0, buttonX, buttonY), "RESTART"))
    	{
			action.RestartGame();
    	}
		if (GUI.Button(new Rect(buttonX*2+60, 0, buttonX, buttonY), "NEXT ROUND"))
		{
			action.NextRound();
		}
		GUIStyle fontStyle = new GUIStyle();
		fontStyle.fontSize = 25;
		GUI.Label(new Rect(0, buttonY + 30, 200, buttonY), "Score: " + action.GetScore(), fontStyle);
		GUI.Label(new Rect(0, buttonY*2+50, 200, buttonY), "Round: " + action.GetRound(), fontStyle);
    }
}
