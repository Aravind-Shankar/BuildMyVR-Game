using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class carController : NetworkBehaviour
{

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

	private bool revOrFor = true;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		//rb.centerOfMass = new Vector3 (0.0f, -0.9f, 0.0f);

		/*LoadMacroState ();*/
	}

	/*
	void OnGUI(){
		GUI.Label (new Rect (10.0f, 10.0f, 50.0f, 50.0f), "Acc Defective: " + isAccDefect);
	}
	*/

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
        if (!isLocalPlayer)
        {
            return;
        }

		// used to inc the torque linearly
		if (accFactor <= 1) {
			accFactor += acceleration * (Time.deltaTime);
		}

		//AccDefect ();
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
				
		/*if (triggerTimer == true) {
			triggerTime += Time.deltaTime;
		} 

		else {
			triggerTime = 0.0f;		
		}*/

		/*if (sceneManager.instance.inSceneTransition)
			SaveMacroState ();*/
	}

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
	}*/

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

	void CarBrakesOn(){
		magState = 1;
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
			wheelRL.motorTorque = accFactor * maxTorque * 0.5f;
			wheelRR.motorTorque = accFactor * maxTorque * 0.5f;
		}
		else {
			wheelRL.motorTorque = -accFactor * maxTorque;
			wheelRR.motorTorque = -accFactor * maxTorque;
		}
	}
}
