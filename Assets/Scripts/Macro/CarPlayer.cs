using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

using Vroom.Networking;

namespace Vroom
{
	namespace InGame
	{
		[RequireComponent (typeof(Rigidbody))]
		public class CarPlayer : NetworkBehaviour
		{
		
			public TextMesh nameHUDText;
		
			public Text nameFPText;
			public Text rankText;
			public Text speedText;
		
			public GameObject finishCanvas;
			public Text finishRankMessage;
			public Text finishWaitMessage;
		
			public float speedScaleFactor = 10f;
			// acc for model scaling
		
			//[HideInInspector]
			[SyncVar]
			public int cpCount;
			//[HideInInspector]
			[SyncVar]
			public float distToNextCP;
		
			//[HideInInspector]
			[SyncVar]
			public string playerName;
			//[HideInInspector]
			[SyncVar]
			public int index;
		
			private Rigidbody attachedRigidbody;
			private CustomNetworkLobbyManager manager;
			private int rank;
			private int numPlayers;
		
			private Transform checkpointsParent;
			private Plane[] checkpointPlanes;
		
			private bool triggerEntered, triggerExited;
			private bool finished;

			void Start ()
			{
				attachedRigidbody = GetComponent<Rigidbody> ();
				manager = NetworkManager.singleton as CustomNetworkLobbyManager;
				if (nameHUDText != null)
					nameHUDText.text = "P" + index + ": " + playerName;
			}

			public override void OnStartLocalPlayer ()
			{
				if (nameFPText != null)
					nameFPText.text = "P" + index + ": " + playerName;
				checkpointsParent = GameObject.Find ("Checkpoints").transform;
				SetUpPlanes ();
			}

			void FixedUpdate ()
			{
				if (!isLocalPlayer || finished)
					return;
				UpdateDistFromNextCP ();
		
				ShowSpeed ();
				CmdQueryRank ();
			}

			void OnTriggerEnter (Collider other)
			{
				if (!isLocalPlayer || finished)
					return;
		
				if (other.CompareTag ("Checkpoint") && !triggerEntered)
					triggerEntered = true;
			}

			void OnTriggerExit (Collider other)
			{
				if (!isLocalPlayer || finished)
					return;
				
				if (other.CompareTag ("Checkpoint") && triggerEntered) {
					triggerEntered = false;
		
					if (triggerExited) {
						triggerExited = false;
						return;
					}
		
					triggerExited = true;
					int cpIndex = other.transform.GetSiblingIndex ();
					TextMesh startText = other.gameObject.GetComponentInChildren<TextMesh> ();
		
					if (cpIndex == cpCount) {
						++cpCount; 
						if (startText != null)
							startText.text = startText.text.Replace ("START", "FINISH");
					} else if (cpIndex == cpCount - 1) {
						--cpCount;
						if (startText != null)
							startText.text = startText.text.Replace ("FINISH", "START");
					} else if (cpIndex == 0 && cpCount == checkpointsParent.childCount) {
						finished = true;
						CmdSignalFinished ();
		
						speedText.enabled = false;
						rankText.enabled = false;
						nameFPText.enabled = false;
						finishCanvas.SetActive (true);
						finishRankMessage.text = "P" + index + ": " + playerName + " - Finished at Position " + rank;
						finishWaitMessage.text = "Please do not quit until all players finish.";
					}
				}
			}

			void SetUpPlanes ()
			{
				var meshFilters = checkpointsParent.GetComponentsInChildren<MeshFilter> ();
				checkpointPlanes = new Plane[meshFilters.Length];
				for (int i = 0; i < meshFilters.Length; ++i) {
					checkpointPlanes [i] = new Plane ();
					checkpointPlanes [i].SetNormalAndPosition (
						meshFilters [i].transform.TransformVector (meshFilters [i].mesh.normals [0]),
						meshFilters [i].transform.position
					);
				}
			}

			void UpdateDistFromNextCP ()
			{
				int nextPlaneIndex = (cpCount == checkpointPlanes.Length) ? 0 : cpCount;
				distToNextCP = Mathf.Abs (checkpointPlanes [nextPlaneIndex].GetDistanceToPoint (transform.position));
			}

			void ShowSpeed ()
			{
				if (speedText != null)
					speedText.text = "Speed: " +
					((int)(attachedRigidbody.velocity.magnitude * (5f / 18f) * speedScaleFactor)) +
					" kmph";
			}

			void ShowRank ()
			{
				if (rankText != null) {
					if (numPlayers == 0)
						CmdQueryNumPlayers ();
					else
						rankText.text = "POS " + rank + " / " + numPlayers;
				}
			}

			void QuitGame ()
			{
				#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
				#endif
				Application.Quit ();
			}

			[Command]
			void CmdQueryNumPlayers ()
			{
				if (isLocalPlayer)
					numPlayers = manager.numPlayers;
				else
					RpcReceiveNumPlayers (manager.numPlayers);
			}

			[Command]
			void CmdQueryRank ()
			{
				int rank = manager.getCarPlayerRank (index);
				if (isLocalPlayer) {
					this.rank = rank;
					ShowRank ();
				} else
					RpcReceiveRank (rank);
			}

			[Command]
			void CmdSignalFinished ()
			{
				if (manager.SignalFinishedAndCheckForOthers (index)) {
				} else {
					RpcQuit ();
				}
			}

			[ClientRpc]
			void RpcReceiveNumPlayers (int num)
			{
				numPlayers = num;
			}

			[ClientRpc]
			void RpcReceiveRank (int rank)
			{
				if (isLocalPlayer) {
					print ("recd rank " + rank);
					this.rank = rank;
					ShowRank ();
				}
			}

			[ClientRpc]
			void RpcQuit ()
			{
				if (isLocalPlayer) {
					finishWaitMessage.text = "All done. Exiting in 5 seconds.";
					Invoke ("QuitGame", 5f);
				}
			}
		}
	}
}
