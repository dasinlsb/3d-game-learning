using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {
    private Director director;
    private FirstController sceneController;
    private void Start() {
        director = Director.getInstance();
        sceneController = director.currentSceneController as FirstController;
    }
    private void Update() {
        if (Input.GetButtonDown("Fire1")) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject ufo = hit.transform.gameObject;                
                sceneController.UFOIsShot(ufo);
            }
        }
    }

}