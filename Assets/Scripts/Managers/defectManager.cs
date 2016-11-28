using UnityEngine;
using System.Collections;

public class defectManager : MonoBehaviour {

	private float defectTimer;
	public GameObject car;
	private carSpeedometer c;
	// Use this for initialization
	void Start () {
		defectTimer = 10.0f;
		c = car.GetComponent<carSpeedometer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		defectTimer -= Time.deltaTime;

		if (defectTimer <= 0.0f) {
			c.isDefect = true;
		}


	}
}
