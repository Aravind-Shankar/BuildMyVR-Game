using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

using Vroom.InGame;

namespace Vroom
{
	namespace Networking
	{
		public class CustomNetworkLobbyManager : NetworkLobbyManager
		{
			public HorizontalLayoutGroup playersPanel;
		
			[HideInInspector]
			public CarPlayer[] carPlayers;
		
			private int numActivePlayers;

			public override bool OnLobbyServerSceneLoadedForPlayer (
				GameObject lobbyPlayer, GameObject gamePlayer)
			{
				CustomLobbyPlayer lobbyPlayerComponent = lobbyPlayer.GetComponent<CustomLobbyPlayer> ();
				CarPlayer carPlayerComponent = gamePlayer.GetComponent<CarPlayer> ();
				carPlayerComponent.playerName = lobbyPlayerComponent.playerName;
				carPlayerComponent.index = lobbyPlayerComponent.slot + 1;
		
				if (carPlayers == null || carPlayers.Length == 0)
					carPlayers = new CarPlayer[numPlayers];
				carPlayers [lobbyPlayerComponent.slot] = carPlayerComponent;
				++numActivePlayers;
				return true;
			}

			public int getCarPlayerRank (int index)
			{
				for (int i = 0; i < numPlayers; ++i)
					if (carPlayers [i] == null)
						return -1;
				carPlayers = carPlayers.OrderBy (p => p.distToNextCP)
					.OrderByDescending (p => p.cpCount)
					.ToArray ();
				int rank = 1;
				for (int i = 0; i < carPlayers.Length; ++i, ++rank)
					if (carPlayers [i].index == index)
						return rank;
				return -1;
			}

			public bool SignalFinishedAndCheckForOthers (int index)
			{
				--numActivePlayers;
				return (numActivePlayers > 0);
			}
		}
	}
}
