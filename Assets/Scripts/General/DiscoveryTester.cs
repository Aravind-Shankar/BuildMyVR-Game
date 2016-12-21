using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class DiscoveryTester : MonoBehaviour {
	public CustomNetworkDiscovery discovery;

	public InputField playerNameField;
	public Text serverStatusText, clientStatusText;

	void OnEnable() {
		discovery.broadcastPort = 7777;
	}

	public void StartAsServer() {
		clientStatusText.text = "Trying to start as server";

		discovery.broadcastData = playerNameField.text;
		discovery.Initialize ();
		if (!discovery.StartAsServer ()) {
			serverStatusText.text = "Unable to start as server";
		} else {
			serverStatusText.text = "Server up with data " + discovery.broadcastData;
		}
	}

	public void StartAsClient() {
		serverStatusText.text = "Trying to start as client";

		discovery.broadcastData = playerNameField.text;
		discovery.Initialize ();
		if (!discovery.StartAsClient ()) {
			clientStatusText.text = "Unable to start as client";
		} else {
			clientStatusText.text = "Listening...";
		}
	}

	public void ReceivedBroadcast(string debugText) {
		clientStatusText.text = debugText;
	}
}
