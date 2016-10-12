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

		public float maxSteer = 15.0f;
		public float maxBrake = 50.0f;


		public float maxTorque = 2000.0f;
		public float acceleration = 0.2f;
		private float accFactor = 0.0f;


		private float triggerTime = 0.0f; //time after which the trigger has been pulled
		private bool triggerTimer; //says whether the timer is on or off
		public float triggerCutoffTime; //time interval for the double click. (triggers the scene change)


		// Use this for initialization
		void Start () {
			rb = GetComponent<Rigidbody> ();
			rb.centerOfMass = new Vector3 (0.0f, -0.9f, 0.0f);
		}

		void OnGUI(){
			GUI.Label (new Rect (5, 5, Screen.width, 30), "Steer Torque: " + wheelFL.steerAngle);
		}

		void FixedUpdate () {
			//used to inc the torque linearly
			if (accFactor <= 1) {
				accFactor += acceleration * (Time.deltaTime);
			} 
				
			wheelRL.motorTorque = -accFactor * maxTorque;
			wheelRR.motorTorque = -accFactor * maxTorque;
			wheelRL.brakeTorque = 0.0f; 
			wheelRR.brakeTorque = 0.0f; 

			//linearly inc the steering angle using accelerometer
			if (Mathf.Abs (Input.acceleration.x) > 1.0f) {
				wheelFL.steerAngle = maxSteer;
				wheelFR.steerAngle = maxSteer;
			} 

			else {
				wheelFL.steerAngle = maxSteer * Input.acceleration.x;
				wheelFR.steerAngle = maxSteer * Input.acceleration.x;
			}




			//this uses the brake
			if (GvrViewer.Instance.Triggered && triggerTime == 0.0f) {
				wheelFL.brakeTorque = maxBrake; 
				wheelFR.brakeTorque = maxBrake;
				triggerTimer = true;
				print ("Triggered");
			} 

			else if (GvrViewer.Instance.Triggered && triggerTime >= triggerCutoffTime) {
				print ("Triggered");
				triggerTimer = false;
			} 

			else if (GvrViewer.Instance.Triggered && (triggerTime < triggerCutoffTime)) { 
				SceneManager.LoadScene ("Jason");
				print ("Triggered");
			}
			

			if (triggerTimer == true) {
				triggerTime += Time.deltaTime;
			} 

			else {
				triggerTime = 0.0f;		
			}
		}
	}

}