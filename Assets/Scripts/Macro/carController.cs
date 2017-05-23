using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Vroom
{
	namespace InGame
	{
		public class CarController : NetworkBehaviour
		{
		
			public WheelCollider wheelFL;
			public WheelCollider wheelFR;
			public WheelCollider wheelRL;
			public WheelCollider wheelRR;
		
			private Rigidbody rb;
		
			public float maxSteer = 20.0f;
			public float maxFBrake = 30.0f;
			public float maxRBrake = 10.0f;
			public float reverseThresholdSpeed = 0.05f;
			public float reverseAccFactor = 0.25f;
		
			public float maxTorque = 1000.0f;
			public float acceleration = 10.0f;
			private float accFactor = 0.0f;
			private float steerInput;
			public bool isAccDefect;
		
			private bool revOrFor = true;
			private bool braking = false;

			void Start ()
			{
				rb = GetComponent<Rigidbody> ();
				//rb.centerOfMass = Vector3.forward * -0.5f;
		
				/*LoadMacroState ();*/
			}

			public override void OnStartLocalPlayer ()
			{
				if (DummyCamController.singleton != null)
					DummyCamController.singleton.DeactivateCamera ();
				gameObject.transform.FindChild ("GvrMain").gameObject.SetActive (true);
				MagnetSensor.OnCardboardTrigger += ToggleBrakes;
			}

			void OnDisable ()
			{
				if (!isLocalPlayer)
					return;
				if (DummyCamController.singleton != null)
					DummyCamController.singleton.ActivateCamera ();
				MagnetSensor.OnCardboardTrigger -= ToggleBrakes;
			}

			void FixedUpdate ()
			{
				if (!isLocalPlayer) {
					return;
				}
		
				//AccDefect ();
				SetMotorTorque ();
				SetSteerAngle ();
				SetBrakeTorque ();
		
				/*if (sceneManager.instance.inSceneTransition)
					SaveMacroState ();*/
			}
		
			// apparently key-press events are handled properly only in Update/LateUpdate
			void Update ()
			{
				if (!isLocalPlayer)
					return;
				
				#if UNITY_EDITOR
				if (Input.GetKeyDown (KeyCode.Space))
					ToggleBrakes ();
				steerInput = Mathf.Clamp (Input.GetAxis ("Horizontal"), -1f, 1f);
				#elif UNITY_ANDROID
				steerInput = Mathf.Clamp(Input.acceleration.x, -1f, 1f) *
							((revOrFor) ? 1f : -1f);
				#endif
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
			void ToggleBrakes ()
			{
				braking = !braking;
			}

			void ToggleDirection ()
			{
				revOrFor = !revOrFor;
				accFactor = 0.0f;
			}

			private void SetMotorTorque ()
			{
				// if moving forward under brakes and velocity falls below thresh,
				// start moving in reverse and stop braking
				if (braking && rb.velocity.magnitude < reverseThresholdSpeed) {
					ToggleDirection ();
					ToggleBrakes ();
				}
		
				// used to inc the torque linearly
				if (accFactor <= 1) {
					accFactor += acceleration * (Time.deltaTime);
				}
		
				if (revOrFor) {
					wheelRL.motorTorque = accFactor * maxTorque;
					wheelRR.motorTorque = accFactor * maxTorque;
				} else {
					wheelRL.motorTorque = -accFactor * reverseAccFactor * maxTorque;
					wheelRR.motorTorque = -accFactor * reverseAccFactor * maxTorque;
				}
			}
			/*
			void CarBrakesOn(){
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
				}
				magState = 1;
				if (rb.velocity.z <= 0.0f && rb.velocity.z >= -0.3f) {
					revOrFor = false;
				} 
			}
			*/
			private void SetSteerAngle ()
			{
				// clamp the steering angle using accelerometer input
				//float inputFactor = Mathf.Clamp(Input.acceleration.x, -1f, 1f);
				wheelFL.steerAngle = maxSteer * steerInput;
				wheelFR.steerAngle = maxSteer * steerInput;
			}

			private void SetBrakeTorque ()
			{
				if (braking) {
					wheelFL.brakeTorque = maxFBrake; 
					wheelFR.brakeTorque = maxFBrake;
					wheelRR.brakeTorque = maxRBrake;
					wheelRL.brakeTorque = maxRBrake;
				} else {
					wheelFL.brakeTorque = 0.0f; 
					wheelFR.brakeTorque = 0.0f;
					wheelRR.brakeTorque = 0.0f;
					wheelRL.brakeTorque = 0.0f;
				}
			}

			public void AccDefect ()
			{
				if (isAccDefect == true) {
					accFactor -= acceleration * Time.deltaTime; 
					if (accFactor < 0.0f) {
						accFactor = 0.0f;
					}
				}
			}
		}
	}
}
