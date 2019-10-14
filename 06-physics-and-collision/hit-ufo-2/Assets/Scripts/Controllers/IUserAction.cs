using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
	void StartGame();
	void RestartGame();
	int GetScore();
	int GetRound();
	void NextRound();
	void SwitchActionMode();
}
