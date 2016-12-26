using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class AntiRollBar : MonoBehaviour {

	public WheelCollider wheelL;
	public WheelCollider wheelR;
	public float antiRollFactor = 5000f;

	private Rigidbody attachedRigidbody;

	void Start() {
		attachedRigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		WheelHit hit = new WheelHit ();
		float travelL = 1f, travelR = 1f;

		bool groundedL = wheelL.GetGroundHit (out hit);
		if (groundedL)
			travelL = (-wheelL.transform.InverseTransformPoint (hit.point).y - wheelL.radius) /
						wheelL.suspensionDistance;

		bool groundedR = wheelR.GetGroundHit (out hit);
		if (groundedR)
			travelR = (-wheelR.transform.InverseTransformPoint (hit.point).y - wheelR.radius) /
						wheelR.suspensionDistance;

		float antiRollForce = (travelL - travelR) * antiRollFactor;
		if (groundedL)
			attachedRigidbody.AddForceAtPosition (wheelL.transform.up * -antiRollForce,
				wheelL.transform.position);
		if (groundedR)
			attachedRigidbody.AddForceAtPosition (wheelR.transform.up * antiRollForce,
				wheelR.transform.position);
	}

}
