using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class checkpointScript : MonoBehaviour {
	public int index;

	public GameObject prevCpObj; //previous checkpoint's gameobject

	private Transform prevCpTrans; //previous checkpoint's transform
	public Transform nextCpTrans; //next checkpoint's transform

	public float distToPrevCp; //used by cc2 for more accuracy

	void start(){
		prevCpTrans = prevCpObj.GetComponent<Transform> ();

		distToPrevCp = (prevCpTrans.position - gameObject.transform.position).magnitude;
	}
}
