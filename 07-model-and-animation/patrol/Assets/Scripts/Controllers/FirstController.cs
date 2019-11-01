using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
  // Prefab
  public Maze mazePrefab;
  public Player playerPrefab;

  private ActionManager actionManager;
  private Director director;
  private PatrolFactory patrolFactory;

  private Player player;
  private Maze maze;
  private List<Patrol> patrols;
  private List<IntVec2> occupiedCoords;

  public enum GameState
  {
    Playing, End, Lose
  }
  public GameState gameState;
  
  void Awake()
  {
    director = Director.getInstance();
    director.currentSceneController = this;
    actionManager = gameObject.AddComponent<ActionManager>();
    LoadResources();
    StartGame();
  }

  public void LoadResources()
  {
    Debug.Log("LoadResources");
  }
  public void Dead ()
  {
    Time.timeScale = 0;
    gameState = GameState.Lose;
  }
  public void EndGame()
  {
    StopAllCoroutines();
    if (maze != null)
    {
      Destroy(maze.gameObject);
    }
    if (player != null)
    {
      Destroy(player.gameObject);
    }
    foreach (Patrol patrol in patrols)
    {
      if (patrol != null)
      {
        Destroy(patrol.gameObject);
      }
    }
    patrols.Clear();
    occupiedCoords.Clear();
    gameState = GameState.End;
  }
  public void RestartGame()
  {
    EndGame();
    StartGame();
  }
  
  public void StartGame() 
  {
    if (Time.timeScale != 1)
    {
      Time.timeScale = 1;
    }
    gameState = GameState.Playing;
    occupiedCoords = new List<IntVec2>();
    maze = Instantiate(mazePrefab) as Maze;
    StartCoroutine(MakeGame());
  }
  public GameState GetGameState()
  {
    return gameState;
  }
  private IEnumerator MakeGame()
  {
    Camera.main.clearFlags = CameraClearFlags.Skybox;
    Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
    maze = Instantiate(mazePrefab) as Maze;
    maze.gameController = this;
    yield return StartCoroutine(maze.Generate());
    player = Instantiate(playerPrefab) as Player;
    occupiedCoords.Add(maze.RandomCoord());
    player.SetLocation(maze.GetCell(occupiedCoords[0]));
    patrolFactory = gameObject.GetComponent<PatrolFactory>();
    patrols = new List<Patrol>();
    for (int i = 0; i < 3; i++) {
      Patrol patrol = patrolFactory.Generate(maze);
      patrol.centerCoord = maze.RandomOtherCoord(occupiedCoords);
      //Debug.Log("patrol init coord: " + patrol.centerCoord);
      occupiedCoords.Add(patrol.centerCoord);
      patrol.SetLocation(maze.GetCell(patrol.centerCoord));
      if (patrol.currentCell.room != player.currentCell.room)
      {
        patrol.gameObject.SetActive(false);
      }
      patrols.Add(patrol);
    }
    Camera.main.clearFlags = CameraClearFlags.Depth;
    Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
  }
}
