using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour {
	static sceneManager instance;

	public string linkedScene1 = "Karthikeswaren";
	public string linkedScene2 = "Jason";

	private float triggerTime = 0.0f; //time after which the trigger has been pulled
	private bool triggerTimer; //says whether the timer is on or off
	public float triggerCutoffTime = 1.0f; //time interval for the double click. (triggers the scene change)

	private Scene currentScene;

	void Start () {
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
	}

	void Update () {
		currentScene = SceneManager.GetActiveScene();

		if (Input.GetKey (KeyCode.Space))
			ChangeScene ();

		if (triggerTimer == true) {
			triggerTime += Time.deltaTime;
		} 

		else {
			triggerTime = 0.0f;		
		}
	}

	void ChangeScene() {
		triggerTimer = true;
		if (triggerTime < triggerCutoffTime){
			triggerTime = 0.0f;
			if (currentScene.name == linkedScene1){
				SceneManager.LoadScene (linkedScene2);
			}

			else if (currentScene.name == linkedScene2){
				SceneManager.LoadScene (linkedScene1);
			}
		}
	}
}
