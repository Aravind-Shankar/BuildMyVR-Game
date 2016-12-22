using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class posManager : MonoBehaviour {
	public GameObject[] p = new GameObject[3];

	private cc2[] c = new cc2[3];

	public Text[] t = new Text[3];

	private Dictionary<GameObject, cc2> players = new Dictionary<GameObject, cc2> ();

	// Use this for initialization
	void Start () {

		c [0] = p [0].GetComponent<cc2> ();
		c [1] = p [1].GetComponent<cc2> ();
		c [2] = p [2].GetComponent<cc2> ();

		players.Add (p[0], c[0]);
		players.Add (p[1], c[1]);
		players.Add (p[2], c[2]);
	}
	/*void OnGUI(){
		GUI.Label (new Rect (10.0f, 10.0f, 50.0f, 50.0f), "First: " + players[players.Keys.ToList()[0]].name);
		GUI.Label (new Rect (10.0f, 20.0f, 50.0f, 50.0f), "Second: " + players[players.Keys.ToList()[1]].name);
		GUI.Label (new Rect (10.0f, 30.0f, 50.0f, 50.0f), "Third: " + players[players.Keys.ToList()[2]].name);
	}*/
	void FixedUpdate () {
		players = players.OrderByDescending (x => x.Value.cpCount).ToDictionary(x => x.Key, x=>x.Value);


		t [0].text = "1st " + players [players.Keys.ToList () [0]].name; 
		t [1].text = "2nd " + players [players.Keys.ToList () [1]].name;
		t [2].text = "3rd " + players [players.Keys.ToList () [2]].name;
	
	}
}
