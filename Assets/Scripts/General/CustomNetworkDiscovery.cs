using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class CustomNetworkDiscovery : NetworkDiscovery {

	//public Text statusText;

	public override void OnReceivedBroadcast(string fromAddress, string data) {
		string debugText = string.Format ("Broadcast received, address {0}, data: {1}",
			                   fromAddress, data); 
		Debug.Log (debugText);
		SendMessage ("ReceivedBroadcast", debugText);
		//statusText.text = debugText;
	}
}
