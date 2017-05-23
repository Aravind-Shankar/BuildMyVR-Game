using UnityEngine;
using System.Collections;

namespace Vroom
{
	namespace Physics
	{
		[RequireComponent (typeof(WheelCollider))]
		public class WheelRotator : MonoBehaviour
		{
		
			private WheelCollider wheel;
			private Transform target;

			void OnEnable ()
			{
				wheel = GetComponent<WheelCollider> ();
				target = transform.GetChild (0);
				target.localRotation = Quaternion.identity;
			}

			void LateUpdate ()
			{
				target.localRotation = Quaternion.Euler (Vector3.forward * wheel.steerAngle);
			}

			void OnDisable ()
			{
				target.localRotation = Quaternion.identity;
			}
		
		}
	}
}
