using UnityEngine;
using System.Collections;

namespace Macro
{
	
	public class keyboardCarController : MonoBehaviour {
		public WheelCollider wheelFL;
		public WheelCollider wheelFR;
		public WheelCollider wheelRL;
		public WheelCollider wheelRR;

		private Rigidbody RB;

		public float maxSteer = 15.0f;
		public float maxFBrake = 50.0f;
		public float maxRBrake = 1.0f;


		public float maxTorque = 500.0f;
		public float acceleration = 0.2f;
		private float accFactor = 0.0f;


		// Use this for initialization
		void Start () {
			RB = GetComponent<Rigidbody> ();
			RB.centerOfMass = new Vector3 (0.0f, -0.9f, 0.0f);
		}

		void OnGUI(){
			GUI.Label (new Rect (5, 5, Screen.width, 30), "Maximum Torque" + maxTorque);
		}

		void FixedUpdate () {

			if (Input.GetAxisRaw ("Vertical") > 0 && accFactor<=1) {
				accFactor += acceleration * (Time.deltaTime);
			} 
			else if (Input.GetAxisRaw ("Vertical") == 0) {
				accFactor = 0.0f;
			} 

			else if (Input.GetAxisRaw ("Vertical") < 0) {
				accFactor =0.4f;
			}


			wheelRL.motorTorque = -accFactor * Input.GetAxisRaw ("Vertical") * maxTorque;
			wheelRR.motorTorque = -accFactor * Input.GetAxisRaw ("Vertical") * maxTorque;
			wheelFL.brakeTorque = 0.0f; 
			wheelFR.brakeTorque = 0.0f; 


			wheelFL.steerAngle = maxSteer * Input.GetAxis ("Horizontal");
			wheelFR.steerAngle = maxSteer * Input.GetAxis ("Horizontal");



			if(Input.GetKey(KeyCode.Space)){
				wheelFL.brakeTorque= maxFBrake; 
				wheelFR.brakeTorque= maxFBrake;
				wheelRL.brakeTorque= maxRBrake;
				wheelRR.brakeTorque= maxRBrake;
			}

		}
	}

}