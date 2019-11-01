﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
	void StartGame();
	void RestartGame();
	FirstController.GameState GetGameState();
}
