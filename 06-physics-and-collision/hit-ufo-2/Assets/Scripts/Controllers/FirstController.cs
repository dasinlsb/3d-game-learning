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
  private ActionManager actionManager;
  private Director director;
  
  public enum GameState
  {
    Playing, Pausing, NotStarted
  }
  public GameState gameState;
  
  void Awake()
  {
    director = Director.getInstance();
    director.currentSceneController = this;
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
    levelManager.CheckScore();
  }

  private void runAction(GameObject ufo, IActionCallback callback)
  {
    var ctl = ufo.GetComponent<UFOController>() as UFOController;
    var action = UFOAction.GetAction(ctl.speed, ctl.direction) as Action;
    action.gameObject = ufo;
    action.transform = ufo.transform;
    action.callback = callback;
    actionManager.AddAction(action);
  }

  public void UFOIsShot(GameObject ufo) 
  {
    levelManager.AddScore(ufo.GetComponent<UFOController>().score);
    visibleUfos.Remove(ufo);
    ufoFactory.Withdraw(ufo);
  }

  public void ActionEvent(Action action)
  {
    GameObject ufo = action.gameObject;
    ufoFactory.Withdraw(ufo);
    visibleUfos.Remove(ufo);
  }

  public void LoadResources() {
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
    foreach(var ufo in visibleUfos)
    {
      levelManager.modifyUfoLevel(ufo);
    }
    gameState = GameState.Playing;
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

  public int GetRound()
  {
    return levelManager.curLevel;
  }

  public int GetScore()
  {
    return levelManager.curScore;
  }

}
