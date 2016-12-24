using UnityEngine;
using System.Collections;

	
	public class keyboardCarController : MonoBehaviour {
				public WheelCollider wheelFL;
				public WheelCollider wheelFR;
				public WheelCollider wheelRL;
				public WheelCollider wheelRR;

				private Rigidbody RB;

				public float maxSteer = 15.0f;
				public float maxFBrake = 50.0f;
				public float maxRBrake = 1.0f;
	public float distErr = 0.0f;

				public float maxTorque = 500.0f;
				public float acceleration = 0.2f;
				private float accFactor = 0.0f;

				private checkpointScript cps;
				private checkpointScript cpsPrev;
				public int numberOfCP = 0;
				public int cpCount = 0;
				public float totDistTra = 0.0f;
				public float distFromCp = 0.0f;
				public GameObject c;
				private bool presOrPrev = true;
	private float prevDFC = 0.0f;
	private Vector3 tr;

				// Use this for initialization
				void Start () {
		tr = GetComponent<Transform> ().position;
					RB = GetComponent<Rigidbody> ();
					RB.centerOfMass = new Vector3 (0.0f, -0.9f, 0.0f);
					numberOfCP = c.transform.childCount;
		distErr = gameObject.GetComponent<BoxCollider> ().size.z;
		distErr = (gameObject.transform.localScale.z) * distErr;
				}
				
				/*
				void OnGUI(){
					GUI.Label (new Rect (5, 5, Screen.width, 30), "Maximum Torque" + wheelRR.motorTorque);
				}
				*/
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
					wheelRL.brakeTorque = 0.0f;
					wheelRR.brakeTorque = 0.0f;

					wheelFL.steerAngle = maxSteer * Input.GetAxis ("Horizontal");
					wheelFR.steerAngle = maxSteer * Input.GetAxis ("Horizontal");



					if(Input.GetKey(KeyCode.Space)){
						wheelFL.brakeTorque = maxFBrake; 
						wheelFR.brakeTorque = maxFBrake;
						wheelRL.brakeTorque = maxRBrake;
						wheelRR.brakeTorque = maxRBrake;
					}

					FindCpDist();
				}

				private void FindCpDist(){
					if (presOrPrev == true) {
			distFromCp = Vector3.Dot (Vector3.Normalize(cps.nextCpTrans.position - cps.transform.position), gameObject.transform.position - cps.transform.position);
						distFromCp = Mathf.Abs (distFromCp);
						distFromCp = distFromCp - distErr;
					}

					else {
			distFromCp = Vector3.Dot (Vector3.Normalize(cpsPrev.nextCpTrans.position - cpsPrev.transform.position), gameObject.transform.position - cpsPrev.transform.position);
						distFromCp = Mathf.Abs (distFromCp);
						distFromCp = distFromCp - distErr;
					}
				}

	void OnTriggerExit(Collider other) {
		if(other.CompareTag ("Checkpoint")){
			cps = other.GetComponent<checkpointScript> ();
			int i = cps.index;

			if (i == cpCount) {
				cpCount += 1;
				totDistTra = totDistTra + distFromCp; 
				prevDFC = distFromCp;


				distFromCp = 0.0f;
				presOrPrev = true;
			}

			else if (i == cpCount-1){
				cpCount -= 1;
				totDistTra = totDistTra - prevDFC;
			
				presOrPrev = false;
				cpsPrev = cps.prevCpObj.GetComponent<checkpointScript> ();
				FindCpDist ();
				prevDFC = distFromCp;
			}

			if (i == 0) {
				totDistTra = 0.0f;
			}

		}
	}
}