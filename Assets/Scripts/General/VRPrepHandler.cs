using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

using Vroom.Networking;

namespace Vroom
{
	namespace PreGame
	{
		public class VRPrepHandler : MonoBehaviour
		{
			public GameObject backButtonCanvas;
			public LobbyHandler lobbyHandler;
		
			private GameObject magnetNotice, successText;

			void Awake ()
			{
				magnetNotice = transform.FindChild ("Magnet Notice").gameObject;
				successText = transform.FindChild ("Success Text").gameObject;
			}

			void OnEnable ()
			{
				backButtonCanvas.SetActive (true);
				Button backButton = backButtonCanvas.GetComponentInChildren<Button> ();
				backButton.onClick.RemoveAllListeners ();
				backButton.onClick.AddListener (GoBack);
		
				MagnetSensor.OnCardboardTrigger += MarkReady;
				magnetNotice.SetActive (true);
				successText.SetActive (false);
			}
		
			#if UNITY_EDITOR
			void Update ()
			{
				if (Input.GetKeyDown (KeyCode.Space))
					MarkReady ();
			}
			#endif
		
			public void MarkReady ()
			{
				if (!CustomLobbyPlayer.localPlayer.readyToBegin) {
					magnetNotice.SetActive (false);
					successText.SetActive (true);
		
					CustomLobbyPlayer.localPlayer.SendReadyToBeginMessage ();
				}
			}

			public void GoBack ()
			{
				if (isActiveAndEnabled) {
					if (CustomLobbyPlayer.localPlayer.readyToBegin) {
						magnetNotice.SetActive (true);
						successText.SetActive (false);
		
						CustomLobbyPlayer.localPlayer.SendNotReadyToBeginMessage ();
					}
					lobbyHandler.TransitionFromVR ();
				}
			}

			void OnDisable ()
			{
				MagnetSensor.OnCardboardTrigger -= MarkReady;
			}
		
		}
	}
}
