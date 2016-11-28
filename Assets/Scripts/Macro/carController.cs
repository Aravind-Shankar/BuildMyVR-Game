using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Macro
{

	public class carController : MonoBehaviour {

		public WheelCollider wheelFL;
		public WheelCollider wheelFR;
		public WheelCollider wheelRL;
		public WheelCollider wheelRR;

		private Rigidbody rb;

		public float maxSteer = 20.0f;
		public float maxFBrake = 50.0f;
		public float maxRBrake = 10.0f;

		public float maxTorque = 1000.0f;
		public float acceleration = 0.2f;
		private float accFactor = 0.0f;

		private float triggerTime = 0.0f; //time after which the trigger has been pulled
		private bool triggerTimer; //says whether the timer is on or off
		public float triggerCutoffTime = 1.0f; //time interval for the double click. (triggers the scene change)


		// Use this for initialization
		void Start () {
			MagnetSensor.OnCardboardTrigger += CarBrakes;
			rb = GetComponent<Rigidbody> ();
			rb.centerOfMass = new Vector3 (0.0f, -0.9f, 0.0f);
		}

		void OnGUI(){

		}

		void FixedUpdate () {
			//used to inc the torque linearly
			if (accFactor <= 1) {
				accFactor += acceleration * (Time.deltaTime);
			}

			wheelRL.motorTorque = -accFactor * maxTorque;
			wheelRR.motorTorque = -accFactor * maxTorque;

			//linearly inc the steering angle using accelerometer
			if (Mathf.Abs (Input.acceleration.x) > 1.0f) {
				wheelFL.steerAngle = maxSteer;
				wheelFR.steerAngle = maxSteer;
			} 

			else {
				wheelFL.steerAngle = maxSteer * Input.acceleration.x;
				wheelFR.steerAngle = maxSteer * Input.acceleration.x;
			}
				
			if (triggerTimer == true) {
				triggerTime += Time.deltaTime;
			} 

			else {
				triggerTime = 0.0f;		
			}
		}
	

		void CarBrakes(){
			if (triggerTime == 0.0f) {
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
			}
		}
	}
}