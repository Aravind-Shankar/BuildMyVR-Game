using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerTester : MonoBehaviour {

	public Text detectionText;
	public float displayTimeSeconds = 0.5f;

	void OnEnable() {
		MagnetSensor.OnCardboardTrigger += SignalTriggered;	
	}

	void OnDisable() {
		MagnetSensor.OnCardboardTrigger -= SignalTriggered;
	}

	void SignalTriggered() {
		detectionText.text = "Triggered";
		StartCoroutine (ResetTriggerText());
	}

	IEnumerator ResetTriggerText() {
		yield return new WaitForSeconds (displayTimeSeconds);
		detectionText.text = "Idle";
	}
}
