using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour {

	public GameObject gamefinderCanvas;
	public GameObject backButtonCanvas;

	void OnEnable() {
		backButtonCanvas.SetActive (false);
	}

	public void HitPlay() {
		gameObject.SetActive (false);
		gamefinderCanvas.SetActive (true);
	}

	public void HitQuit() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
		Application.Quit ();
	}

}
