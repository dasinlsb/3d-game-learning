using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
  private UFOFactory ufoFactory;
  private List<GameObject> visibleUfos;
  private int[] maxAxisLen = new int[]{10, 10, 10};
  private int curLevel;
  private int curScore;
  public bool levelUp;
  public enum GameState
  {
    Playing, Pausing, NotStarted
  }
  public GameState gameState;
  void Awake()
  {
    SSDirector.getInstance().currentSceneController = this;
    LoadResources();
    gameState = GameState.NotStarted;
  }

  void Update()
  {
    if (gameState != GameState.Playing)
    {
      return;
    }
    List<GameObject> invisibleUfos = new List<GameObject>();
    foreach(var obj in visibleUfos) 
    {
      if (!isInView(obj.transform.position))
      {
        invisibleUfos.Add(obj);
        //Debug.Log("one ufo went outside");
      }
    }
    foreach (var ufo in invisibleUfos) 
    {
      ufoFactory.Withdraw(ufo);
      visibleUfos.Remove(ufo);
    }
    if (Input.GetButtonDown("Fire1")) 
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit))
      {        
        GameObject ufo = hit.transform.gameObject;
        curScore = curScore + ufo.GetComponent<UFOController>().score;
        this.visibleUfos.Remove(ufo);
        ufoFactory.Withdraw(ufo);
      }
    }
    for(; visibleUfos.Count < curLevel + 2;)
    {
      GameObject newUfo = ufoFactory.Launch(this.curLevel);
      modifyUfoLevel(newUfo);
      visibleUfos.Add(newUfo);
    }
    if (curScore > curLevel * 10)
    {
      levelUp = true;
      curScore = 0;
      NextRound();
    }
  }


  private bool isInView(Vector3 position) 
  {
    Vector3 viewpos = Camera.main.WorldToViewportPoint(position);
    return viewpos.x > 0.0f && viewpos.x < 1.0f &&
          viewpos.y > 0.0 && viewpos.y < 1.0f;
  }

  public void LoadResources() {
    levelUp = false;
    curLevel = 1;
    curScore = 0;
    ufoFactory = Singleton<UFOFactory>.Instance;
    visibleUfos = new List<GameObject>();
  }

  /**
    *  IUserActions
    */
  public void NextRound()
  {
    gameState = GameState.Pausing;
    curLevel = curLevel + 1;
    foreach(var ufo in visibleUfos)
    {
      modifyUfoLevel(ufo);
    }
    gameState = GameState.Playing;
  }

  public void modifyUfoLevel(GameObject ufo) 
  {
    var random = new System.Random(Guid.NewGuid().GetHashCode());
    Func<int, int> rndn = (n) => { return random.Next(0, n); };
    Func<int, int> rndpn = (n) => { return random.Next(0, n*2) - n; };
    ufo.transform.position = new Vector3(rndn(maxAxisLen[0]), rndn(maxAxisLen[1]), rndn(maxAxisLen[2]));
    var ufoCtl = ufo.GetComponent<UFOController>();
//    Debug.Log("will change speed to :" + curLevel);
    ufoCtl.speed = (float)curLevel *0.05f;
    ufoCtl.direction = new Vector3(rndpn(10), rndpn(10), rndpn(10));
    int ufoKind = rndn(ufoFactory.ufoKinds);
    float scale = ufoFactory.ufoScales[ufoKind];
    ufo.transform.localScale = new Vector3(scale, scale, scale);
    ufo.GetComponent<Renderer>().material.color = ufoFactory.ufoColors[ufoKind];
    ufoCtl.score = ufoFactory.ufoScores[ufoKind];
  }

  public void RestartGame() 
  {
    gameState = GameState.Pausing;
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
    return curLevel;
  }

  public int GetScore()
  {
    return curScore;
  }

}
