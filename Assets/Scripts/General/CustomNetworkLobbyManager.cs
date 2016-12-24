using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class CustomNetworkLobbyManager : NetworkLobbyManager {

	public HorizontalLayoutGroup playersPanel;

	public override bool OnLobbyServerSceneLoadedForPlayer(
		GameObject lobbyPlayer, GameObject gamePlayer)
	{
		CustomLobbyPlayer lobbyPlayerComponent = lobbyPlayer.GetComponent<CustomLobbyPlayer> ();
		CarPlayer carPlayerComponent = gamePlayer.GetComponent<CarPlayer> ();
		carPlayerComponent.playerName = lobbyPlayerComponent.playerName;
		carPlayerComponent.index = lobbyPlayerComponent.slot + 1;
		return true;
	}
}
