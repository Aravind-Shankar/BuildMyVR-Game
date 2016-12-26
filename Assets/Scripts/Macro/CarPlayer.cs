using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CarPlayer : NetworkBehaviour {

	public Text nameHUDText;
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
	public string playerName;
	[HideInInspector]
	public int index;

	private Rigidbody attachedRigidbody;
	private CustomNetworkLobbyManager manager;
	private int rank;
	private int numPlayers;

	private Transform checkpointsParent;
	private Plane[] checkpointPlanes;

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
		print ("query rank");
		CmdQueryRank ();
	}

	void OnTriggerExit(Collider other) {
		if (!isLocalPlayer)
			return;
		
		if (other.CompareTag ("Checkpoint")) {
			int cpIndex = other.transform.GetSiblingIndex ();

			if (cpIndex == cpCount) {
				++cpCount; 
			}
			else if (cpIndex == cpCount - 1) {
				--cpCount;
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
		distToNextCP = Mathf.Abs (checkpointPlanes[cpCount].GetDistanceToPoint(transform.position));
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
