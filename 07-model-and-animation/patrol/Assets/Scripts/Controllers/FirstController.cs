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

  private Player player;
  private Maze maze;
  private List<GameObject> patrols;

  public enum GameState
  {
    Playing, NotStarted
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
  }
  public void RestartGame()
  {
    Debug.Log("Restart");
    EndGame();
    StartGame();
  }
  
  public void StartGame() 
  {
    Debug.Log("Start");
    maze = Instantiate(mazePrefab) as Maze;
    StartCoroutine(MakeGame());
  }
  private IEnumerator MakeGame()
  {
    Camera.main.clearFlags = CameraClearFlags.Skybox;
    Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
    maze = Instantiate(mazePrefab) as Maze;
    yield return StartCoroutine(maze.Generate());
    player = Instantiate(playerPrefab) as Player;
    player.SetLocation(maze.GetCell(maze.RandomCoord()));
    Camera.main.clearFlags = CameraClearFlags.Depth;
    Camera.main.rect = new Rect(0f, 0f, 0.4f, 0.4f);
  }
}
