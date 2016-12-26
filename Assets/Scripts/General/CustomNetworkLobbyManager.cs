using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

public class CustomNetworkLobbyManager : NetworkLobbyManager {

	public HorizontalLayoutGroup playersPanel;

	private CarPlayer[] carPlayers;

	public override bool OnLobbyServerSceneLoadedForPlayer(
		GameObject lobbyPlayer, GameObject gamePlayer)
	{
		CustomLobbyPlayer lobbyPlayerComponent = lobbyPlayer.GetComponent<CustomLobbyPlayer> ();
		CarPlayer carPlayerComponent = gamePlayer.GetComponent<CarPlayer> ();
		carPlayerComponent.playerName = lobbyPlayerComponent.playerName;
		carPlayerComponent.index = lobbyPlayerComponent.slot + 1;

		if (carPlayers == null || carPlayers.Length == 0)
			carPlayers = new CarPlayer[numPlayers];
		carPlayers [lobbyPlayerComponent.slot] = carPlayerComponent;
		return true;
	}

	public int getCarPlayerRank(int index) {
		for (int i = 0; i < numPlayers; ++i)
			if (carPlayers [i] == null)
				return -1;
		carPlayers = carPlayers.OrderBy(p => p.distToNextCP)
			.OrderByDescending (p => p.cpCount)
			.ToArray();
		int rank = 1;
		for (int i = 0; i < carPlayers.Length; ++i, ++rank)
			if (carPlayers [i].index == index)
				return rank;
		return -1;
	}
}
