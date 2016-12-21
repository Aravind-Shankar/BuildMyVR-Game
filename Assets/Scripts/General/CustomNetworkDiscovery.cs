using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class CustomNetworkDiscovery : NetworkDiscovery {

	//public Text statusText;

	public override void OnReceivedBroadcast(string fromAddress, string data) {
		//StopBroadcast ();

		string debugText = string.Format ("Broadcast received, address {0}, data: {1}",
			                   fromAddress, data); 
		Debug.Log (debugText);
		SendMessage ("ReceivedBroadcast", debugText);
		//statusText.text = debugText;

		// fromAddress format is "::ffff:IP"
		NetworkManager.singleton.networkAddress =
			fromAddress.Substring (fromAddress.LastIndexOf (':') + 1);

		if (NetworkManager.singleton.client == null) {
			NetworkManager.singleton.StartClient ();
			Debug.Log ("starting as client");
		}
	}
}
