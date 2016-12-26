using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CarPlayer : NetworkBehaviour {

	public TextMesh nameHUDText;
	public Text rankText;
	public Text speedText;

	public float speedScaleFactor = 10f;	// acc for model scaling

	[HideInInspector]
	[SyncVar]
	public int cpCount;
	[HideInInspector]
	[SyncVar]
	public float distToNextCP;

	[HideInInspector]
	[SyncVar]
	public string playerName;
	[HideInInspector]
	[SyncVar]
	public int index;

	private Rigidbody attachedRigidbody;
	private CustomNetworkLobbyManager manager;
	private int rank;
	private int numPlayers;

	private Transform checkpointsParent;
	private Plane[] checkpointPlanes;

	private bool triggerEntered, triggerExited;

	void Start() {
		attachedRigidbody = GetComponent<Rigidbody> ();
		manager = NetworkManager.singleton as CustomNetworkLobbyManager;
	}

	public override void OnStartLocalPlayer ()
	{
		if (nameHUDText != null)
			nameHUDText.text = "P" + index + ": " + playerName;
		checkpointsParent = GameObject.Find ("Checkpoints").transform;
		SetUpPlanes ();
	}

	void FixedUpdate() {
		if (!isLocalPlayer)
			return;
		UpdateDistFromNextCP ();

		ShowSpeed ();
		CmdQueryRank ();
	}

	void OnTriggerEnter(Collider other) {
		if (!isLocalPlayer)
			return;

		if (other.CompareTag ("Checkpoint") && !triggerEntered)
			triggerEntered = true;
	}

	void OnTriggerExit(Collider other) {
		if (!isLocalPlayer)
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
				print ("checkpoints crossed: " + cpCount);
				if (startText != null)
					startText.text = startText.text.Replace ("START", "FINISH");
			}
			else if (cpIndex == cpCount - 1) {
				--cpCount;
				print ("checkpoints crossed: " + cpCount);
				if (startText != null)
					startText.text = startText.text.Replace ("FINISH", "START");
			}
			else if (cpIndex == 0 && cpCount == checkpointsParent.childCount) {
				#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
				#endif
				Application.Quit ();
			}
		}
	}

	void SetUpPlanes() {
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

	void UpdateDistFromNextCP () {
		int nextPlaneIndex = (cpCount == checkpointPlanes.Length) ? 0 : cpCount;
		distToNextCP = Mathf.Abs (checkpointPlanes[nextPlaneIndex].GetDistanceToPoint(transform.position));
	}

	void ShowSpeed () {
		if (speedText != null)
			speedText.text = "Speed: " + 
				((int) (attachedRigidbody.velocity.magnitude * (5f / 18f) * speedScaleFactor)) +
				" kmph";
	}

	void ShowRank() {
		if (rankText != null) {
			if (numPlayers == 0)
				CmdQueryNumPlayers ();
			else
				rankText.text = "POS " + rank + " / " + numPlayers;
		}
	}

	[Command]
	void CmdQueryNumPlayers () {
		if (isLocalPlayer)
			numPlayers = manager.numPlayers;
		else
			RpcReceiveNumPlayers (manager.numPlayers);
	}

	[Command]
	void CmdQueryRank() {
		int rank = manager.getCarPlayerRank (index);
		if (isLocalPlayer) {
			this.rank = rank;
			ShowRank ();
		}
		else
			RpcReceiveRank(rank);
	}

	[ClientRpc]
	void RpcReceiveNumPlayers(int num) {
		numPlayers = num;
	}

	[ClientRpc]
	void RpcReceiveRank(int rank) {
		if (isLocalPlayer) {
			print ("recd rank " + rank);
			this.rank = rank;
			ShowRank ();
		}
	}
}
