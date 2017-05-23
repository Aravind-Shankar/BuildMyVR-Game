using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

using Vroom.PreGame;

namespace Vroom
{
	namespace Networking
	{
		public class CustomNetworkDiscovery : NetworkDiscovery
		{
			public const char END_MARK = (char)1;

			public static string ParseData (string sentData)
			{
				// data is name + END_MARK + garbage
				return sentData.Remove (sentData.IndexOf (END_MARK)).Trim ();
			}

			public static string ParseIP (string sentAddress)
			{
				// fromAddress format is "::ffff:IP"
				return sentAddress.Substring (sentAddress.LastIndexOf (':') + 1);
			}

			void ClearDict ()
			{
				broadcastsReceived.Clear ();
				SendMessage ("ClearedDict", SendMessageOptions.DontRequireReceiver);
			}

			public new bool StartAsClient ()
			{
				if (base.StartAsClient ()) {
					InvokeRepeating ("ClearDict", 30f, 30f);
					return true;
				}
				return false;
			}

			public override void OnReceivedBroadcast (string fromAddress, string data)
			{
				if (GameFinder.instance != null)
					GameFinder.instance.ReceivedBroadcast (ParseData (data), ParseIP (fromAddress));
			}

			public new void StopBroadcast ()
			{
				if (running) {
					base.StopBroadcast ();
					CancelInvoke ();
				}
			}
		}
	}
}
