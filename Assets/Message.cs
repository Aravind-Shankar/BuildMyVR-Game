using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
		TextMesh textObject = GameObject.Find("DefectText").GetComponent<TextMesh>();
		textObject.text = "";



	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
}
