using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class DiscoveryTester : MonoBehaviour {
	public CustomNetworkDiscovery discovery;

	public InputField playerNameField;
	//public Text serverStatusText, clientStatusText;

	private CustomNetworkLobbyManager manager;

	void OnEnable() {
		discovery.broadcastPort = 7777;
		manager = NetworkManager.singleton as CustomNetworkLobbyManager;
	}

	public void StartAsHost() {
		//clientStatusText.text = "Trying to start as host";

		discovery.broadcastData = discovery.broadcastPort + ":" + playerNameField.text;
		discovery.Initialize ();
		if (!discovery.StartAsServer ()) {
			//serverStatusText.text = "Unable to start as host";
		} else {
			manager.StartHost ();
			//serverStatusText.text = "Server up with data " + discovery.broadcastData;
		}
	}

	public void StartAsClient() {
		//serverStatusText.text = "Trying to start as client";

		discovery.Initialize ();
		if (!discovery.StartAsClient ()) {
			//clientStatusText.text = "Unable to start as client";
		} else {
			//clientStatusText.text = "Listening...";
		}
	}

	public void ReceivedBroadcast(string debugText) {
		//clientStatusText.text = debugText;
	}

	void OnDisable() {
		if (discovery.running)
			discovery.StopBroadcast ();
	}
}
