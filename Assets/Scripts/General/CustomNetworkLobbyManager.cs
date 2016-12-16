using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CustomNetworkLobbyManager : NetworkLobbyManager {

	public override bool OnLobbyServerSceneLoadedForPlayer(
		GameObject lobbyPlayer, GameObject gamePlayer)
	{
		LobbyPlayer lobbyPlayerComponent = lobbyPlayer.GetComponent<LobbyPlayer> ();
		CarPlayer carPlayerComponent = gamePlayer.GetComponent<CarPlayer> ();

		return true;
	}

}
