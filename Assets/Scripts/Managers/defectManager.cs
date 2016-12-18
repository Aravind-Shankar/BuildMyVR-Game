using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class defectManager : MonoBehaviour {

	private float speedometerDefectTime;
	private float accelerationDefectTime;
	public GameObject car;
	private carSpeedometer s;
	private cc2 c;
	private Random r;
	private static int l;
							
	// Use this for initialization
	void Start () {
		
		speedometerDefectTime = 10.0f * nonRepRandom (1, 5);
		s = car.GetComponent<carSpeedometer> ();
		speedometerDefectTime += (10.0f) * (Random.value);

		print (speedometerDefectTime);

		accelerationDefectTime = 10.0f * nonRepRandom (1, 5);
		c = car.GetComponent<cc2> ();
		accelerationDefectTime += (10.0f) * (Random.value);

		print (accelerationDefectTime);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		speedometerDefectTime -= Time.deltaTime;

		if (speedometerDefectTime <= 0.0f) {
			s.isDefect = true;
		}
		else if (accelerationDefectTime <= 0.0f) {
			c.isAccDefect = true;
		}
	}

	public static int nonRepRandom(int min, int max){
		int a = Random.Range (min, max);

		if (a == l) {
			return nonRepRandom (min, max);
		}
		l = a;
		return a;
	}
}
