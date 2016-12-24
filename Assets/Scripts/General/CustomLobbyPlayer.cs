using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class CustomLobbyPlayer : NetworkLobbyPlayer {

	[HideInInspector]
	public string playerName = "";

	[HideInInspector]
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

	public IEnumerator SetUpUI(bool canBeLocalPlayer) {
		if (canBeLocalPlayer) {
			yield return new WaitForEndOfFrame ();
			if (isLocalPlayer) {
				readyText.text = "YOU";
				readyText.fontStyle = FontStyle.BoldAndItalic;

				playerName = GameFinder.instance.nameField.text;
			} else {
				yield return new WaitForEndOfFrame ();
				foreach (CustomLobbyPlayer player in lobby.lobbySlots) {
					if (player != null)
						player.SendYourName ();
				}
				readyText.text = (readyToBegin) ? "READY" : "Not Ready";
			}
		}
		else
			readyText.text = (readyToBegin) ? "READY" : "Not Ready";
		
		nameText.text = (slot + 1) + ". " + playerName;
		yield return null;
	}

	public void SendYourName() {
		if (isLocalPlayer) {
			CmdSendName (playerName);
		}
	}

	[Command]
	void CmdSendName(string outName) {
		RpcReceiveName (outName);
	}

	[ClientRpc]
	void RpcReceiveName(string recdName) {
		if (!isLocalPlayer && playerName == "") {
			playerName = recdName;
			StartCoroutine (SetUpUI (false));
		}
	}

}
