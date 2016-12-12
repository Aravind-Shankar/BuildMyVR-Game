using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class DummyCamController : MonoBehaviour {

	private Camera attachedCamera;

	void OnEnable() {
		attachedCamera = GetComponent<Camera> ();
	}

	void Update() {
		if (NetworkManager.singleton != null) {
			attachedCamera.enabled = !NetworkManager.singleton.isNetworkActive;
			attachedCamera.tag = (attachedCamera.enabled) ? "MainCamera" : "Untagged";
		}
	}
}
