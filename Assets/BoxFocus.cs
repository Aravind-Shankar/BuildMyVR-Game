using UnityEngine;
using System.Collections;

public class BoxFocus : MonoBehaviour {



	public Renderer rend;
	public bool isDefect;
	// Use this for initialization
	void Start () {

		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Focus()
	{
		
		rend.material.color = new Color (0f, 100f, 0f, 0f);
		if (isDefect) {
			rend.material.color = new Color (100f, 0f, 0f, 0f);
		}
	}
	public void Ignore()
	{
		rend.material.color = Color.white; 

	}

}
