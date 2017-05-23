using UnityEngine;
using System.Collections;

namespace Vroom
{
	namespace PreGame
	{
		public class SplashController : MonoBehaviour
		{
			public Camera eventCamera;
			public string camPathName;
			public float camMoveTime;
		
			public GameObject carVR;
			public string carPathName;
			public float carMoveTime;
		
			public float intervalTime;
		
			public Animator mainMenuAnimator;
			public GameObject backButtonCanvas;

			void OnEnable ()
			{
				backButtonCanvas.SetActive (false);
			}

			public void StartMovement ()
			{
				this.eventCamera.clearFlags = CameraClearFlags.Skybox;
				iTween.MoveTo (this.eventCamera.gameObject, iTween.Hash (
					"path", iTweenPath.GetPath (this.camPathName),
					"time", camMoveTime,
					"easeType", iTween.EaseType.easeInOutSine
				));
				StartCoroutine (WaitAndStartCar ());
			}

			IEnumerator WaitAndStartCar ()
			{
				yield return new WaitForSeconds (intervalTime);
				yield return StartCarWithMenu ();
			}

			public IEnumerator StartCarWithMenu ()
			{
				iTween.MoveTo (carVR, iTween.Hash (
					"path", iTweenPath.GetPath (this.carPathName),
					"time", carMoveTime
				));
				yield return new WaitForSeconds (carMoveTime);
				mainMenuAnimator.SetTrigger ("startPlaying");
				yield return null;
			}
		}
	}
}
