using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour {
	static sceneManager instance;
	// Use this for initialization

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
	
	// Update is called once per frame
	void Update () {

		currentScene = SceneManager.GetActiveScene();

		/*
		if (Input.GetKey(KeyCode.Space) && (triggerTime < triggerCutoffTime)) { 
			SceneManager.LoadScene ("Jason");
			print ("Triggered");
		}
		*/
		if (Input.GetKey(KeyCode.Space)){
			triggerTimer = true;
			if (triggerTime < triggerCutoffTime){
				triggerTime = 0.0f;
				if (currentScene.name == "Karthikeswaren"){
					SceneManager.LoadScene ("Jason");
				}

				else if (currentScene.name == "Jason"){
					SceneManager.LoadScene ("Karthikeswaren");
				}
			}
		}

		if (triggerTimer == true) {
			triggerTime += Time.deltaTime;
		} 

		else {
			triggerTime = 0.0f;		
		}
	}
}
