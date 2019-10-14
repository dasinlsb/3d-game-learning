using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
public class FirstController : MonoBehaviour, ISceneController, IUserAction, IActionCallback
{
  private UFOFactory ufoFactory;
  private Shooter shooter;
  private LevelManager levelManager;
  private List<GameObject> visibleUfos;
  private ActionAdapter actionAdapter;
  private ActionManager actionManager;
  private Director director;

  private List<Action> startedActions;
  private bool willSwitchActionMode;
  
  public enum GameState
  {
    Playing, Pausing, NotStarted
  }
  public GameState gameState;
  
  void Awake()
  {
    director = Director.getInstance();
    director.currentSceneController = this;
    actionAdapter = new ActionAdapter();
    actionManager = gameObject.AddComponent<ActionManager>();
    ufoFactory = Singleton<UFOFactory>.Instance;
    shooter = gameObject.AddComponent<Shooter>();
    levelManager = gameObject.AddComponent<LevelManager>();
    LoadResources();
    gameState = GameState.NotStarted;
  }

  void Update()
  {
    if (gameState != GameState.Playing) return;
    for(; visibleUfos.Count < levelManager.curLevel + 2;)
    {
      GameObject newUfo = ufoFactory.Launch(levelManager.curLevel);
      levelManager.modifyUfoLevel(newUfo);
      visibleUfos.Add(newUfo);
      runAction(newUfo, this);
    }
  }

  private void runAction(GameObject ufo, IActionCallback callback)
  {
    var action = actionAdapter.MakeAction(ufo, callback);
    startedActions.Add(action);
    actionManager.AddAction(action);
  }

  public void UFOIsShot(GameObject ufo) 
  {
    levelManager.curScore += ufo.GetComponent<UFOController>().score;
    visibleUfos.Remove(ufo);
    ufoFactory.Withdraw(ufo);
    var actions = startedActions.FindAll(action => action.gameObject == ufo);
    if (actions.Count != 0) 
    {
      actionManager.RemoveAction(actions[0]);
    }
    levelManager.CheckScore();
  }

  public void ActionEvent(Action action)
  {
    GameObject ufo = action.gameObject;
    ufoFactory.Withdraw(ufo);
    visibleUfos.Remove(ufo);
  }

  public void LoadResources() {
    willSwitchActionMode = false;
    startedActions = new List<Action>();
    visibleUfos = new List<GameObject>();
  }

  /**
    *  IUserActions
    */
  public void NextRound()
  {
    if (gameState == GameState.NotStarted) return;
    gameState = GameState.Pausing;
    levelManager.LevelUp();
    updateActionMode();
    foreach(var ufo in visibleUfos)
    {
      ufoFactory.Withdraw(ufo);
    }
    visibleUfos.Clear();
    gameState = GameState.Playing;
  }

  private void updateActionMode()
  {
    if (willSwitchActionMode)
    {
      willSwitchActionMode = false;
      actionAdapter.SwitchActionMode();    
    }
  }

  public void RestartGame() 
  {
    gameState = GameState.Pausing;
    levelManager.Reset();
    foreach (var ufo in visibleUfos)
    {
      ufoFactory.Withdraw(ufo);
    }
    visibleUfos.Clear();
    gameState = GameState.NotStarted;
    StartGame();
  }
  public void StartGame()
  {
    if (gameState != GameState.NotStarted)
    {
      return;
    }
    gameState = GameState.NotStarted;
    LoadResources();
    gameState = GameState.Playing;
  }

  public void SwitchActionMode()
  {
    if (gameState == GameState.NotStarted) return;
    willSwitchActionMode = true;
    NextRound();
  }

  public int GetRound()
  {
    return levelManager.curLevel;
  }

  public int GetScore()
  {
    return levelManager.curScore;
  }

}
