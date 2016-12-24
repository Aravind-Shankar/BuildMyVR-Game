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
		if (!CustomLobbyPlayer.localPlayer.readyToBegin) {
			magnetNotice.SetActive (false);
			successText.SetActive (true);

			CustomLobbyPlayer.localPlayer.SendReadyToBeginMessage ();
		}
	}

	public void GoBack() {
		magnetNotice.SetActive (true);
		successText.SetActive (false);

		CustomLobbyPlayer.localPlayer.SendNotReadyToBeginMessage ();
	}

	void OnDisable() {
		MagnetSensor.OnCardboardTrigger -= MarkReady;
	}

}
