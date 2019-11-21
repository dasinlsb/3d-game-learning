using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
	public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	transform.Rotate(Vector3.up * 2.0f);
    	if (Input.GetButtonDown("Fire1")) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject g = hit.transform.gameObject;
                if (g == gameObject) 
                {
                	slider.value -= 0.05f;
                	Debug.Log("hit myself");
                }
            }
        }

    }
}
