using UnityEngine;
using System.Collections;

public class LobbyHandler : MonoBehaviour {

	public GameObject propCar;

	public void TransitionToVR() {
		gameObject.SetActive (false);
		Camera.main.enabled = false;

		GameObject gvrMain = propCar.transform.FindChild ("GvrMain").gameObject;
		gvrMain.SetActive (true);
	}

}
