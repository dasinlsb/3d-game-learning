using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
	private IUserAction action;
	private const int buttonX = 140;
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
		if (Input.GetKeyDown(KeyCode.Space)) {
			action.RestartGame();
		}
		GUI.Label(new Rect(0, 0, 100, 100), "Notice");
    }
}
