using UnityEngine;
using System.Collections;

public class carSpeedometer : MonoBehaviour {

	private Rigidbody carRB;
	private float carSpeed;
	public bool isDefect = false;
	private string speedText;
	// Use this for initialization
	void Start () {
		carRB = GetComponent<Rigidbody> ();
	}

	void OnGUI(){
		GUI.Label (new Rect (5, 30, Screen.width, 30), "Speed: " + speedText);
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (!isDefect) {
			carSpeed = carRB.velocity.magnitude;
			carSpeed = carSpeed * (5.0f / 18.0f);
			carSpeed = (int)carSpeed;	
			speedText = carSpeed + " kmph";
		}
		else if (isDefect) {
			carSpeed = (int)carSpeed;	
			speedText = carSpeed + " kmph DEFECTIVE";
		}

	}
}
