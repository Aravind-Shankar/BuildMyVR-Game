/*
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

<<<<<<< HEAD


public class carController : NetworkBehaviour
{

       
=======
public class carController : MonoBehaviour {
>>>>>>> refs/remotes/origin/master

	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;

	private Rigidbody rb;

	public float maxSteer = 20.0f;
	public float maxFBrake = 30.0f;
	public float maxRBrake = 10.0f;

	private int magState = 0;

	public float maxTorque = 1000.0f;
	private float acceleration = 100.0f;
	private float accFactor = 0.0f;
	public bool isAccDefect;

	private float triggerTime = 0.0f; //time after which the trigger has been pulled
	private bool triggerTimer; //says whether the timer is on or off
	public float triggerCutoffTime = 1.0f; //time interval for the double click. (triggers the scene change)

	private bool revOrFor = true;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.centerOfMass = new Vector3 (0.0f, -0.9f, 0.0f);

		/*LoadMacroState ();*/
	//}

	/*
	void OnGUI(){
		GUI.Label (new Rect (10.0f, 10.0f, 50.0f, 50.0f), "Acc Defective: " + isAccDefect);
	}
	*/
/*
	void OnEnable() {
		MagnetSensor.OnCardboardTrigger += CarBrakesOn;
	}

	void OnDisable() {
		if (magState == 0) {
			MagnetSensor.OnCardboardTrigger -= CarBrakesOff;
		}
		else if (magState == 1) {
			MagnetSensor.OnCardboardTrigger -= CarBrakesOn;
		}
	}

	void FixedUpdate () {
<<<<<<< HEAD
        if (!isLocalPlayer)
        {
            return;
        }
        wheelFL.brakeTorque = 0.0f; 
		wheelFR.brakeTorque = 0.0f;
		wheelRR.brakeTorque = 0.0f;
		wheelRL.brakeTorque = 0.0f;

=======
		//BrakeAssign ();
>>>>>>> refs/remotes/origin/master
		//used to inc the torque linearly
		//MagnetSensor.OnCardboardTrigger += CarBrakesOn;

		if (accFactor <= 1) {
			accFactor += acceleration * (Time.deltaTime);
		}
		AccDefect ();

		Torque ();

		//linearly inc the steering angle using accelerometer
		if (Mathf.Abs (Input.acceleration.x) > 1.0f) {
			wheelFL.steerAngle = maxSteer;
			wheelFR.steerAngle = maxSteer;
		} 

		else {
			wheelFL.steerAngle = maxSteer * Input.acceleration.x;
			wheelFR.steerAngle = maxSteer * Input.acceleration.x;
		}

		if (Input.GetKey (KeyCode.Space))
			CarBrakesOn ();
				
		if (triggerTimer == true) {
			triggerTime += Time.deltaTime;
		} 

		else {
			triggerTime = 0.0f;		
		}

		/*if (sceneManager.instance.inSceneTransition)
			SaveMacroState ();*/
	//}

	/*void LoadMacroState() {
		MacroState pastState = sceneManager.instance.GlobalMacroState;
		if (pastState != null) {
			transform.position = pastState.carPosition;
			transform.rotation = pastState.carRotation;
		}
	}
	
	void SaveMacroState() {
		MacroState globalState = sceneManager.instance.GlobalMacroState;
		if (globalState == null)
			globalState = sceneManager.instance.GlobalMacroState = new MacroState ();
			
		globalState.carPosition = transform.position;
		globalState.carRotation = transform.rotation;
<<<<<<< HEAD
	}*/
/*
	void CarBrakes(){
=======
	}
	/*
	void BrakeAssign(){
		if (magState == 0) {
			MagnetSensor.OnCardboardTrigger -= CarBrakesOff;
			MagnetSensor.OnCardboardTrigger += CarBrakesOn;
		}
		else if (magState == 1) {
			MagnetSensor.OnCardboardTrigger -= CarBrakesOn;
			MagnetSensor.OnCardboardTrigger += CarBrakesOff;
		}
	}
	*/
/*
	void CarBrakesOn(){
>>>>>>> refs/remotes/origin/master
			/*if (triggerTime == 0.0f) {
				wheelFL.brakeTorque = maxFBrake; 
				wheelFR.brakeTorque = maxFBrake;
				wheelRR.brakeTorque = maxRBrake;
				wheelRL.brakeTorque = maxRBrake;
				triggerTimer = true;

			}
			else if (triggerTime >= triggerCutoffTime) {
				wheelFL.brakeTorque = 0.0f;
				wheelFR.brakeTorque = 0.0f;
				wheelRL.brakeTorque = 0.0f; 
				wheelRR.brakeTorque = 0.0f;
				triggerTime = 0.0f;
				triggerTimer = false;
			}*/
/*		magState = 1;
		if (rb.velocity.z <= 0.0f && rb.velocity.z >= -0.3f) {
			revOrFor = false;
		} 
		else {
			wheelFL.brakeTorque = maxFBrake; 
			wheelFR.brakeTorque = maxFBrake;
			wheelRR.brakeTorque = maxRBrake;
			wheelRL.brakeTorque = maxRBrake;
		}
		MagnetSensor.OnCardboardTrigger -= CarBrakesOn;
		MagnetSensor.OnCardboardTrigger += CarBrakesOff;
	}

	void CarBrakesOff(){
		magState = 0;
		if (!revOrFor) {
			revOrFor = true;
		}
		else {
			wheelFL.brakeTorque = 0.0f; 
			wheelFR.brakeTorque = 0.0f;
			wheelRR.brakeTorque = 0.0f;
			wheelRL.brakeTorque = 0.0f;
		}
		MagnetSensor.OnCardboardTrigger -= CarBrakesOff;
		MagnetSensor.OnCardboardTrigger += CarBrakesOn;
	}

	public void AccDefect(){
		if (isAccDefect == true){
			accFactor -= acceleration * Time.deltaTime; 
			if (accFactor < 0.0f) {
				accFactor = 0.0f;
			}
		}
	}

	private void Torque (){
		if (revOrFor) {
			wheelRL.motorTorque = -accFactor * maxTorque;
			wheelRR.motorTorque = -accFactor * maxTorque;
		}
		else {
			wheelRL.motorTorque = accFactor * maxTorque;
			wheelRR.motorTorque = accFactor * maxTorque;
		}
	}
}
*/