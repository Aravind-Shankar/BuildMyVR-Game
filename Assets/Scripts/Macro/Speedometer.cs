using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Macro
{
	
	public class Speedometer : MonoBehaviour {
		public Rigidbody target;
		private Text speedText;
		// Use this for initialization
		void Start () {
			
			speedText=GetComponent<Text>();
		}
		
		// Update is called once per frame
		void FixedUpdate () {
			
		 	double speed = target.velocity.magnitude;
		 	speed = speed * (5.0 / 18.0);
		 	speedText.text = speed + " kmph";
		}
	}

}