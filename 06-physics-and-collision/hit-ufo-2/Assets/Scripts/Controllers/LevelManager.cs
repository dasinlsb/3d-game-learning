using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour {
    private int[] maxAxisLen = new int[]{10, 10, 10};
    private UFOFactory ufoFactory;
    private FirstController sceneController;

    public int curLevel;
    public int curScore;
    private void Start() {
        sceneController = Director.getInstance().currentSceneController as FirstController;
        ufoFactory = Singleton<UFOFactory>.Instance;
        Reset();
    }
    public void modifyUfoLevel(GameObject ufo) 
    {
        var random = new System.Random(Guid.NewGuid().GetHashCode());
        Func<int, int> rndn = (n) => { return random.Next(0, n); };
        Func<int, int> rndpn = (n) => { return random.Next(0, n*2) - n; };
        ufo.transform.position = new Vector3(rndn(maxAxisLen[0]), rndn(maxAxisLen[1]), rndn(maxAxisLen[2]));
        var ufoCtl = ufo.GetComponent<UFOController>();
        ufoCtl.speed = new Vector3(rndpn(10), rndpn(10), rndpn(10)) * (0.05f * (float)curLevel);
        int ufoKind = rndn(ufoFactory.ufoKinds);
        float scale = ufoFactory.ufoScales[ufoKind];
        ufo.transform.localScale = new Vector3(scale, scale, scale);
        ufo.GetComponent<Renderer>().material.color = ufoFactory.ufoColors[ufoKind];
        ufoCtl.score = ufoFactory.ufoScores[ufoKind];
    }
    public void Reset() {
        curScore = 0;
        curLevel = 1;
    }
    public void CheckScore()
    {
        if (curScore > curLevel * 10) {
            LevelUp();
            sceneController.NextRound();
        }
    }
    public void LevelUp()
    {
        curScore = 0;
        curLevel = curLevel + 1;
    }
    public void AddScore(int addn)
    {
        curScore += addn;
    }
    
}
 
