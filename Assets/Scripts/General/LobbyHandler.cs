using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

using Vroom.Networking;

namespace Vroom
{
	namespace PreGame
	{
		public class LobbyHandler : MonoBehaviour
		{
		
			public Camera mainCamera;
			public GameObject propCar;
			public GameObject backButtonCanvas;
			public GameObject gamefinderCanvas;
		
			private GameObject gvrMain;

			void OnEnable ()
			{
				backButtonCanvas.SetActive (false);
				Button backButton = backButtonCanvas.GetComponentInChildren<Button> ();
				backButton.onClick.RemoveAllListeners ();
				backButton.onClick.AddListener (GoBack);
		
				gvrMain = propCar.transform.FindChild ("GvrMain").gameObject;
			}

			public void TransitionToVR ()
			{
				gameObject.SetActive (false);
				mainCamera.enabled = false;
				gvrMain.SetActive (true);
			}

			public void TransitionFromVR ()
			{
				gvrMain.SetActive (false);
				gameObject.SetActive (true);
				mainCamera.enabled = true;
			}

			public void GoBack ()
			{
				if (isActiveAndEnabled) {
					CustomLobbyPlayer.localPlayer.RemovePlayer ();
					gameObject.SetActive (false);
					gamefinderCanvas.SetActive (true);
				}
			}
		
		}
	}
}
