using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
	void Restart();
	void BootBoat();
	void PriestUpBoat();
	void DevilUpBoat();
	void PriestDownBoat();
	void DevilDownBoat();
    FirstController.GameState GetGameState();
}
