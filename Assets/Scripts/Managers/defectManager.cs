using UnityEngine;
using System.Collections;

public class defectManager : MonoBehaviour {

	private float defectTime;
	public GameObject car;
	private carSpeedometer c;
	// Use this for initialization
	void Start () {
		defectTime = 10.0f;
		c = car.GetComponent<carSpeedometer> ();
		defectTime += (10.0f) * (Random.value);
		print (defectTime);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		defectTime -= Time.deltaTime;

		if (defectTime <= 0.0f) {
			c.isDefect = true;
		}


	}
}
