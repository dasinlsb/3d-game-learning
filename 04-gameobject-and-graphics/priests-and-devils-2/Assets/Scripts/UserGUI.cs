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
        action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    // Update is called once per frame
    void OnGUI() {
    	if (GUI.Button(new Rect(0, 0, buttonX, buttonY), "Restart"))
    	{
    		action.Restart();
    	}
    	FirstController.GameState state = action.GetGameState();
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
    	if (state == FirstController.GameState.Win) 
        {
    		GUI.Label(new Rect(gridX, 0, 200, 100), "YOU WIN !");
    	}
        else if (state == FirstController.GameState.Lose) 
        {
    		GUI.Label(new Rect(gridX, 0, 200, 100), "YOU LOSE !");
    	} 
        else 
        {
            bool isAnimating = state == FirstController.GameState.Animating;
            GUI.enabled = !isAnimating;
    		// continueing...
        	if (GUI.Button(new Rect(0, gridY, buttonX, buttonY), "Send a devil to boat"))
        	{
        		action.DevilUpBoat();
        	}
        	if (GUI.Button(new Rect(gridX,gridY, buttonX, buttonY), "Send a priest to boat"))
        	{
        		action.PriestUpBoat();
        	}
        	if (GUI.Button(new Rect(2*gridX,gridY, buttonX, buttonY), "Let a devil leave boat"))
        	{
        		action.DevilDownBoat();
        	}
        	if (GUI.Button(new Rect(3*gridX, gridY, buttonX, buttonY) , "Let a priest leave boat"))
        	{
        		action.PriestDownBoat();
        	}
        	if (GUI.Button(new Rect(0, 2*gridY, buttonX, buttonY), "GO !"))
        	{
        		action.BootBoat();
        	}
        }
    }
}
