using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour {
	public static sceneManager instance;

	public string linkedScene1 = "Karthikeswaren";
	public string linkedScene2 = "Jason";

	[HideInInspector]
	public MacroState GlobalMacroState = null;
	[HideInInspector]
	public bool inSceneTransition = false;

	private float triggerTime = 0.0f; //time after which the trigger has been pulled
	private bool triggerTimer; //says whether the timer is on or off
	public float triggerCutoffTime = 1.0f; //time interval for the double click. (triggers the scene change)

	private Scene currentScene;

	void Awake () {					// called before Start(), OnEnable() of all scripts
		if (instance != null) {
			Destroy (gameObject);
		} 
		else {
			DontDestroyOnLoad (gameObject);
			instance = this;
		}
	}

	void OnEnable() {
		MagnetSensor.OnCardboardTrigger += ChangeScene;
	}

	void OnDisable() {
		MagnetSensor.OnCardboardTrigger -= ChangeScene;
		GlobalMacroState = null;
	}

	void Update () {
		currentScene = SceneManager.GetActiveScene();

		if (Input.GetKeyDown (KeyCode.Space) && !inSceneTransition)
			ChangeScene ();

		if (triggerTimer == true) {
			triggerTime += Time.deltaTime;
			if (triggerTime > triggerCutoffTime) {
				triggerTimer = false;
				triggerTime = 0.0f;
			}
		} 

		else {
			triggerTime = 0.0f;		
		}
	}

	void ChangeScene() {
		if (!triggerTimer)
			triggerTimer = true;
		else if (triggerTime < triggerCutoffTime){
			triggerTime = 0.0f;
			if (currentScene.name == linkedScene1){
				StartCoroutine(SignalAndSwitchScenes(linkedScene2));
			}

			else if (currentScene.name == linkedScene2){
				StartCoroutine(SignalAndSwitchScenes(linkedScene1));
			}
		}
	}

	IEnumerator SignalAndSwitchScenes(string sceneName) {
		inSceneTransition = true;
		triggerTimer = false;
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();		// REQUIRED, ensures that all updates happen
		inSceneTransition = false;
		SceneManager.LoadScene (sceneName);
	}
}
