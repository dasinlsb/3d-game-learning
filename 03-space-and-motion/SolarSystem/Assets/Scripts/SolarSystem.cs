using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SolarSystem : MonoBehaviour
{
	public const int starNum = 9;
	public List<GameObject> stars;
	private int sunId = 8;
	private float disSunEarth = 149597870; // km
	private float disOffset = 1f / 300000000;
	private float sizeOffset = 1f / 100000;
	private float rateOffset = 1f * 300;
	
	private string[] names = {
		"水星",
		"火星",
		"天王星",
		"金星",
		"土星",
		"地球",
		"海王星",
		"木星",
		"太阳"
	};
	private double[] selfRadius = { // km
		2440,
		3397,
		15559, //25559,
		6052,
		30268, //60268,
		6378,
		24764,
		35492, //71492,
		80000 // 69600
	};
	private double[] rotRadius = { // disSunEarth km
		0.3871,
		1.5237,
		12, //19.2184,
		0.7233,
		9.5549,
		1,
		20, //30.1104,
		5.2026,
		0
	};
	private double[] selfPeriod = { // minute
		59 * 24,
		24,
		17,
		243 * 24,
		10,
		23,
		16,
		9,
		32
	};
	private double[] rotPeriod = { // day
		87.97,
		687,
		84.01 * 365,
		225,
		29.46 * 365,
		365.26,
		164.82 * 365,
		11.86 * 365,
		0
	};

    // Start is called before the first frame update
    void renderCircles(float width) {
    	var pts = new List<Vector3>();
		LineRenderer line = gameObject.AddComponent<LineRenderer>();
		for (int i = 0; i < starNum; i++) {
			int pcnt = 100;
			for (int j = 0; j < pcnt; j++) {
				float angle = 2.0f * Mathf.PI * j / (float)pcnt;
				float dis = (float)rotRadius[i] * disSunEarth * disOffset;
				pts.Add(new Vector3(dis*Mathf.Cos(angle), 0, dis*Mathf.Sin(angle)));
			}
		}
		line.SetWidth(width, width);
		line.positionCount = pts.Count;
		line.SetPositions(pts.ToArray()); 
    }
    void renderStars() {
		for (int i = 0; i < starNum; i++) {
        	stars.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
        	stars[i].transform.position = new Vector3((float)rotRadius[i] * disSunEarth * disOffset, 0, 0);
        	float rad = (float)selfRadius[i] * sizeOffset;
        	stars[i].transform.localScale = new Vector3 (rad, rad, rad);
        }
    }
    void Start()
    { 
    	renderCircles(0.003f);
		renderStars();
        
    }

    // Update is called once per frame
    void Update()
    {
    	for (int i = 0; i < stars.Count; i++) {
    		if (i == sunId) {
    			stars[i].transform.RotateAround(stars[sunId].transform.position, new Vector3(0, 1, 0), 0);
    		} else {
    			stars[i].transform.RotateAround(stars[sunId].transform.position, new Vector3(0, 1, 0),
    								rateOffset * 365 * Time.deltaTime / (float)rotPeriod[i]);
    		}
    	}
    }

}
