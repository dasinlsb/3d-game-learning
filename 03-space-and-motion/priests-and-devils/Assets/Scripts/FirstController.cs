using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
	private const int cntDevil = 3;
	private const int cntPriest = 3;
	private const int lenB = 8;
	private Vector3 groundPosSrc = new Vector3(-4, 0, 0);
	private Vector3 groundPosDst   = new Vector3(4, 0, 0);
	private Vector3 groundPosLeft  = new Vector3(0, 0, 1);
	private Vector3 groundPosRight = new Vector3(0, 0, -1);
	private Vector3 boatPosSrc     = new Vector3(-2.5f, 1, 0);
	private Vector3 boatPosDst     = new Vector3(2.5f, 1, 0);
	private Vector3 rolePosSrc     = new Vector3(-4.5f, 1, 0);
	private const float boatSpeed      = 4.0f;
	private const float spacingOffset  = 0.2f;
	// private Vector3 rolsPosDst     = new Vector3(4.5f, 1, 0);

	private GameObject river, groundSrc, groundDst, groundLeft, groundRight, boat;
	private List<GameObject> sideSrc, sideDst, passers;
	private GameState gameState;
	private MoveState boatState;
	public enum GameState {
		Playing, Win, Lose
	}
	public enum MoveState {
		AtSrc, AtDst, FromSrc, FromDst
	}
    // Start is called before the first frame update
    void Awake()
    {
    	SSDirector.getInstance().currentSceneController = this;
    	LoadResources();
    }

    // Update is called once per frame
    void Update()
    {
        switch (boatState)
        {
        	case MoveState.FromSrc:
        		boat.transform.position = Vector3.MoveTowards(boat.transform.position, boatPosDst, boatSpeed * Time.deltaTime);
        		break;
        	case MoveState.FromDst:
        		boat.transform.position = Vector3.MoveTowards(boat.transform.position, boatPosSrc, boatSpeed * Time.deltaTime);
        		break;
        }
        if (boat.transform.position == boatPosSrc) 
        {
        	boatState = MoveState.AtSrc;
        }
        if (boat.transform.position == boatPosDst)
        {
        	boatState = MoveState.AtDst;
        }
    }


    /**
     *  Utility
     */

    private List<GameObject> findRoles(List<GameObject> list, string name)
    {
    	return list.FindAll(role => role.name == name);
    }

    /**
     *	ISceneController
     */

    public void LoadResources() {
    	gameState = GameState.Playing;
    	boatState = MoveState.AtSrc;
    	river = Instantiate(Resources.Load<GameObject>("Prefabs/River"));
    	groundSrc   = Instantiate(Resources.Load<GameObject>("Prefabs/Ground"), groundPosSrc,   Quaternion.identity);
    	groundDst   = Instantiate(Resources.Load<GameObject>("Prefabs/Ground"), groundPosDst,   Quaternion.identity);
    	groundLeft  = Instantiate(Resources.Load<GameObject>("Prefabs/Ground"), groundPosLeft,  Quaternion.identity);
    	groundRight = Instantiate(Resources.Load<GameObject>("Prefabs/Ground"), groundPosRight, Quaternion.identity);
    	boat = Instantiate(Resources.Load<GameObject>("Prefabs/Boat"), boatPosSrc, Quaternion.Euler(90, 90, 0));
    	sideSrc = new List<GameObject>();
    	sideDst = new List<GameObject>();
    	passers = new List<GameObject>();
    	for (var i = 0; i < cntDevil; i++) {
    		Vector3 p = rolePosSrc - new Vector3(1+spacingOffset, 0, 0) * i;
    		var devil = Instantiate(Resources.Load<GameObject>("Prefabs/Devil"), p, Quaternion.identity);
    		devil.name = "Devil";
    		sideSrc.Add(devil);
    	}
    	for (var i = 0; i < cntPriest; i++) {
    		Vector3 p = rolePosSrc - new Vector3(1+spacingOffset, 0, 0) * (i+3);
    		var priest = Instantiate(Resources.Load<GameObject>("Prefabs/Priest"), p, Quaternion.identity);
    		priest.name = "Priest";
    		sideSrc.Add(priest);
    	}
    }

    /**
   	 *  IUserActions
   	 */

   	public void Restart() 
   	{
   		Destroy(river);
   		Destroy(groundSrc);
   		Destroy(groundDst);
   		Destroy(groundLeft);
   		Destroy(groundRight);
   		Destroy(boat);
   		foreach (var i in sideSrc) {
   			Destroy(i);
   		}
   		foreach (var i in sideDst) {
   			Destroy(i);
   		}
   		foreach (var i in passers) {
   			Destroy(i);
   		}
   		// Destroy(sideSrc);
   		// Destroy(sideDst);
   		// Destroy(passer);
   		LoadResources();
   	}

   	public void BootBoat()
   	{
   		// if (gameState != GameState.Playing) return;
   		switch (boatState) 
   		{
   			case MoveState.AtSrc:
   				boatState = MoveState.FromSrc;
   				break;
   			case MoveState.AtDst:
   				boatState = MoveState.FromDst;
   				break;
   			default:
   				break;
   		}
   	}
   	public void DownBoat(string name) 
   	{
   		// if (gameState != GameState.Playing) return;
   		if (boatState != MoveState.AtDst) return;
   		var list = findRoles(passers, name);
   		if (list.Count == 0) return;
   		var role = list[0];
   		role.transform.parent = null;
   		role.transform.localPosition = groundPosDst + 
   						new Vector3(0, 1+spacingOffset) * (sideDst.Count + 1);
   		sideDst.Add(role);
   		passers.Remove(role);
   		updateGameState();
   	}
   	public void UpBoat(string name) 
   	{
   		// if (gameState != GameState.Playing) return;
   		if (boatState != MoveState.AtSrc) return;
   		if (passers.Count >= 2) return;
   		var list = findRoles(sideSrc, name);
   		
   		if (list.Count == 0) return;
   		var role = list[0];
   		role.transform.parent = boat.transform;
   		role.transform.localPosition = boatPosSrc + new Vector3(2.5f, -1, 0) +
   						new Vector3(0, 0, -1f-spacingOffset) * (passers.Count + 1);
   		passers.Add(role);
   		sideSrc.Remove(role);
   		updateGameState();
   	}
   	public void DevilUpBoat()
   	{
   		UpBoat("Devil");
   	}
   	public void PriestUpBoat()
   	{
   		UpBoat("Priest");
   	}
   	public void DevilDownBoat()
   	{
   		DownBoat("Devil");
   	}
   	public void PriestDownBoat()
   	{
   		DownBoat("Priest");
   	}
   	private void updateGameState()
   	{
   		if (findRoles(sideSrc, "Priest").Count > 0 &&
   			findRoles(sideSrc, "Devil").Count > findRoles(sideSrc, "Priest").Count) {
   			gameState = GameState.Lose;
   		}
   		if (findRoles(sideDst, "Priest").Count > 0 &&
   			findRoles(sideDst, "Devil").Count > findRoles(sideDst, "Priest").Count) {
   			gameState = GameState.Lose;
   		}
   		if (sideDst.Count == cntPriest + cntDevil) {
   			gameState = GameState.Win;
   		}
   	}
   

    public GameState GetGameState() {
    	return gameState;
    }
}
