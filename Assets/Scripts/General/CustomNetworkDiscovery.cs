using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkDiscovery : NetworkDiscovery {
    private string dataString, addressString;
   
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        dataString = data;
        addressString = fromAddress;
        
    }

    void OnGUI() {
        dataString = GUI.TextField(new Rect(10, 10, 200, 20),dataString, 25);
        if (GUI.Button(new Rect(10, 70, 200, 20), "Connect"))
        {
            NetworkManager.singleton.networkAddress = addressString;
            NetworkManager.singleton.StartClient();
        }
            
    }
}
