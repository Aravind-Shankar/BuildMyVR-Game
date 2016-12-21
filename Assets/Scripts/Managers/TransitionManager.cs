using UnityEngine;
using System.Collections;

public class TransitionManager : MonoBehaviour {

	public static TransitionManager instance;

	public SplashFields splashFields;

	void Awake () {					// called before Start(), OnEnable() of all scripts
		if (instance != null) {
			Destroy (gameObject);
		} 
		else {
			DontDestroyOnLoad (gameObject);
			instance = this;
		}
	}

	void Start() {
		if (splashFields != null) {
			// in splash scene
			if (splashFields.splashShown) {
				splashFields.splashAnimator.enabled = false;
				splashFields.splashCanvas.enabled = false;

				Camera.main.transform.position = splashFields.cameraEndPoint;

				StartCoroutine(splashFields.splashController.StartCarWithMenu ());
			}
			else
				splashFields.splashShown = true;
		}
	}

	[System.Serializable]
	public class SplashFields {
		[HideInInspector]
		public bool splashShown;

		public Vector3 cameraEndPoint;
		public Canvas splashCanvas;
		public Animator splashAnimator;
		public SplashController splashController;
	}
}
