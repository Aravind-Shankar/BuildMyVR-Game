using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class VRPrepHandler : MonoBehaviour {

	private GameObject magnetNotice, successText;

	void Awake() {
		magnetNotice = transform.FindChild ("Magnet Notice").gameObject;
		successText = transform.FindChild ("Success Text").gameObject;
	}

	void OnEnable() {
		MagnetSensor.OnCardboardTrigger += MarkReady;
		magnetNotice.SetActive (true);
		successText.SetActive (false);
	}

	#if UNITY_EDITOR
	void Update() {
		if (Input.GetKeyDown (KeyCode.Space))
			MarkReady ();
	}
	#endif

	public void MarkReady() {
		magnetNotice.SetActive (false);
		successText.SetActive (true);
	}

	public void GoBack() {
		magnetNotice.SetActive (true);
		successText.SetActive (false);
	}

	void OnDisable() {
		MagnetSensor.OnCardboardTrigger -= MarkReady;
	}

}
