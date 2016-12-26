using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class cc2 : MonoBehaviour {
	
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;

	private Rigidbody rb;

	public float maxSteer = 20.0f;

	private int magState = 0;

	private checkpointScript cps;
	private checkpointScript cpsPrev;
	public int numberOfCP = 0;
	public int cpCount = 0;
	public float totDistTra = 0.0f;
	public float distFromCp = 0.0f;
	public GameObject c;
	private bool presOrPrev = true;
	public float distErr = 0.0f;
	private float prevDFC = 0.0f;

	public float maxTorque = 1000.0f;
	private float acceleration = 70.0f;
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

		numberOfCP = c.transform.childCount;
		distErr = gameObject.GetComponent<BoxCollider> ().size.z;
		distErr = (gameObject.transform.localScale.z) * distErr;

		LoadMacroState ();
	}

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
		//used to inc the torque linearly
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

		if (sceneManager.instance.inSceneTransition) {
			SaveMacroState ();
		}

		FindCpDist();

	}

	void LoadMacroState() {
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
	}

	void CarBrakesOn(){
		magState = 1;
		revOrFor = false;

		MagnetSensor.OnCardboardTrigger -= CarBrakesOn;
		MagnetSensor.OnCardboardTrigger += CarBrakesOff;
	}

	void CarBrakesOff(){
		magState = 0;
		revOrFor = true;
		
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

	private void FindCpDist(){
		if (presOrPrev == true) {
			distFromCp = Vector3.Dot (Vector3.Normalize(cps.nextCpTrans.position - cps.presCpTrans.position), gameObject.transform.position - cps.presCpTrans.position);
			distFromCp = Mathf.Abs (distFromCp);
			distFromCp = distFromCp - distErr;
		}

		else {
			distFromCp = Vector3.Dot (Vector3.Normalize(cpsPrev.nextCpTrans.position - cpsPrev.presCpTrans.position), gameObject.transform.position - cpsPrev.presCpTrans.position);
			distFromCp = Mathf.Abs (distFromCp);
			distFromCp = distFromCp - distErr;
		}
	}

	private void Torque (){
		if (revOrFor) {
			wheelRL.motorTorque = -accFactor * maxTorque;
			wheelRR.motorTorque = -accFactor * maxTorque;
		}
		else {
			wheelRL.motorTorque = accFactor * maxTorque * 0.5f;
			wheelRR.motorTorque = accFactor * maxTorque * 0.5f;
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

			if (i >= numberOfCP - 1) {
				print ("You have finished");
			}

		}
	}
}
