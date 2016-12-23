using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class CustomNetworkDiscovery : NetworkDiscovery {

	public static string parseIP(string sentAddress) {
		return sentAddress.Substring (sentAddress.LastIndexOf (':') + 1);
	}

	void ClearDict() {
		broadcastsReceived.Clear ();
		SendMessage ("ClearedDict", SendMessageOptions.DontRequireReceiver);
	}

	public new bool StartAsClient() {
		if (base.StartAsClient ()) {
			InvokeRepeating ("ClearDict", 30f, 30f);
			return true;
		}
		return false;
	}

	public override void OnReceivedBroadcast(string fromAddress, string data) {
		HostInfo info = new HostInfo ();
		// fromAddress format is "::ffff:IP"
		info.hostIP = parseIP(fromAddress);
		// data is just the name, but apparently control characters get appended
		data = data.Trim();
		int controlCharStart = 0;
		while (controlCharStart < data.Length && !char.IsControl (data [controlCharStart]))
			++controlCharStart;
		info.hostName = data.Remove(controlCharStart);
		SendMessage ("ReceivedBroadcast", info, SendMessageOptions.DontRequireReceiver);
	}

	public new void StopBroadcast() {
		base.StopBroadcast ();
		CancelInvoke ();
	}

	public struct HostInfo {
		public string hostName;
		public string hostIP;
	}
}
