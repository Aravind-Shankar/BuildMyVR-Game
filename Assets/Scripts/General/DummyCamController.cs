using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class DummyCamController : MonoBehaviour {

	public static DummyCamController singleton;

	private Camera attachedCamera;

	void Awake() {
		if (singleton != null)
			Destroy (gameObject);
		else
			singleton = this;
	}

	void OnEnable() {
		attachedCamera = GetComponent<Camera> ();
	}

	// old version checks for any network activity
	/*void Update() {
		if (NetworkManager.singleton != null) {
			attachedCamera.enabled = !NetworkManager.singleton.isNetworkActive;
			attachedCamera.tag = (attachedCamera.enabled) ? "MainCamera" : "Untagged";
		}
	}*/

	// new version is fired only when another camera is ready
	public void DeactivateCamera() {
		attachedCamera.enabled = false;
		attachedCamera.tag = "Untagged";
	}

	// activation function just in case
	public void ActivateCamera() {
		attachedCamera.enabled = true;
		attachedCamera.tag = "MainCamera";
	}
}
