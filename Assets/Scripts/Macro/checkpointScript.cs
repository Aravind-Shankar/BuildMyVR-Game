using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class checkpointScript : MonoBehaviour {
	public int index; //start from zero

	public GameObject prevCpObj; //previous checkpoint's gameobject

	public Transform nextCpTrans; //next checkpoint's transform

	[HideInInspector]
	public Transform presCpTrans; // present checkpoint's transform. Don't initialise this.

	void start(){
		presCpTrans = GetComponent<Transform> ();
	}
}
