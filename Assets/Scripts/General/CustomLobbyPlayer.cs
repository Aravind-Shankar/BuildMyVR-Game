using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class CustomLobbyPlayer : NetworkLobbyPlayer {

	public static CustomLobbyPlayer localPlayer;

	//[HideInInspector]
	public string playerName = "";

	//[HideInInspector]
	public Text nameText, readyText;

	private CustomNetworkLobbyManager lobby;

	public override void OnClientEnterLobby ()
	{
		lobby = NetworkManager.singleton as CustomNetworkLobbyManager;
		Transform assignedPanel = lobby.playersPanel.transform.GetChild (slot);
		nameText = assignedPanel.GetChild (0).gameObject.GetComponent<Text> ();
		readyText = assignedPanel.GetChild (1).gameObject.GetComponent<Text> ();

		StartCoroutine (SetUpUI (true));
	}

	public override void OnClientExitLobby ()
	{
		if (isLocalPlayer) {
			localPlayer = null;
		}
		if (nameText != null)
			nameText.text = "";
		if (readyText != null)
			readyText.text = "";
	}

	public override void OnClientReady (bool readyState)
	{
		readyToBegin = readyState;
		AssignReadyText ();
	}

	void AssignReadyText() {
		readyText.text = (isLocalPlayer) ? "YOU" : (
			(readyToBegin) ? "READY" : "Not Ready"
		);
	}

	public IEnumerator SetUpUI(bool canBeLocalPlayer) {
		if (canBeLocalPlayer) {
			yield return new WaitForEndOfFrame ();
			if (isLocalPlayer) {
				readyText.fontStyle = FontStyle.BoldAndItalic;

				playerName = GameFinder.instance.nameField.text;
				localPlayer = this;
			}
			else {
				yield return new WaitForEndOfFrame ();
				foreach (CustomLobbyPlayer player in lobby.lobbySlots) {
					if (player != null)
						player.SendStats ();
				}
			}
		}

		AssignReadyText ();
		nameText.text = (slot + 1) + ". " + playerName;
		yield return null;
	}

	public void SendStats() {
		if (isLocalPlayer) {
			CmdSendStats (playerName, readyToBegin);
		}
	}

	[Command]
	void CmdSendStats(string outName, bool outReadyState) {
		RpcReceiveStats (outName, outReadyState);
	}

	[ClientRpc]
	void RpcReceiveStats(string recdName, bool recdReadyState) {
		if (!isLocalPlayer) {
			playerName = recdName;
			readyToBegin = recdReadyState;
			StartCoroutine (SetUpUI (false));
		}
	}

}
