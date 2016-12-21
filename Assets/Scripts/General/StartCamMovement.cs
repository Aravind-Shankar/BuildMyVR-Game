using UnityEngine;
using System.Collections;

public class StartCamMovement : MonoBehaviour {

	public Camera camera;
	public string camPathName;
	public float camMoveTime;

	public GameObject carVR;
	public string carPathName;
	public float carMoveTime;

	public float intervalTime;

	public void StartMovement() {
		this.camera.clearFlags = CameraClearFlags.Skybox;
		iTween.MoveTo (this.camera.gameObject, iTween.Hash (
			"path", iTweenPath.GetPath(this.camPathName),
			"time", camMoveTime,
			"easeType", iTween.EaseType.easeInOutSine
		));
		StartCoroutine (WaitAndStartCar());
	}

	IEnumerator WaitAndStartCar() {
		yield return new WaitForSeconds(intervalTime);
		iTween.MoveTo(carVR, iTween.Hash(
			"path", iTweenPath.GetPath(this.carPathName),
			"time", carMoveTime
		));
		yield return null;
	}
}
