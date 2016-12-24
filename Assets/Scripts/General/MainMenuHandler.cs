using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour {

	public GameObject gameFinderCanvas;

	public void HitPlay() {
		gameObject.SetActive (false);
		gameFinderCanvas.SetActive (true);
	}

	public void HitQuit() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
		Application.Quit ();
	}

}
