using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Movement: MonoBehaviour {

	public GameObject Head;
	public float speed=1.0f;
	public float timeFactor;
	//private float t=0.0f;
	private Rigidbody rb;
	private bool flag;
	private Vector3 InitialVelocity=Vector3.zero;
	private Vector3 FinalVelocity;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.freezeRotation = true;
		flag = false; 
		timeFactor = 5.0f;
	}

	void OnEnable() {
		MagnetSensor.OnCardboardTrigger += ToggleMovement;
	}

	void OnDisable() {
		MagnetSensor.OnCardboardTrigger -= ToggleMovement;
	}
		
	void FixedUpdate () {
		FinalVelocity = speed * Head.transform.forward;
		if (flag) {
			rb.velocity = Vector3.Lerp(InitialVelocity,FinalVelocity,Time.deltaTime*timeFactor);

			if(Time.deltaTime*timeFactor<=1)
			timeFactor += 5.0f;
		}
		else {
			rb.velocity=Vector3.zero;
			timeFactor = 5.0f;

		}
		
		if (Input.GetKey (KeyCode.Space))
			ToggleMovement();

	}

	void ToggleMovement(){
		flag = !flag;
	}
}
