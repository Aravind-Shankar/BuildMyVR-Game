using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(CustomNetworkDiscovery))]
public class GameFinder : MonoBehaviour {
	public static GameFinder instance;

	public GridLayoutGroup activeHostsGrid;
	public GameObject activeHostButtonPrefab;
	public Button hostButton;
	public Button joinButton;
	public InputField nameField;
	public Text hostNameText;

	public Canvas lobbyCanvas;

	[HideInInspector]
	public Dictionary<string, string> activeHosts;
	[HideInInspector]
	public string selectedHostName = "";
	private Button selectedButton;
	private bool clearingGrid;

	private CustomNetworkDiscovery discovery;

	void Awake() {
		if (instance != null) {
			Destroy (gameObject);
		} 
		else {
			instance = this;
		}
	}

	void OnEnable() {
		discovery = GetComponent<CustomNetworkDiscovery> ();
		activeHosts = new Dictionary<string, string> (StringComparer.InvariantCulture);

		discovery.Initialize ();
		discovery.StartAsClient ();

		hostButton.onClick.AddListener (
			() => {
				hostButton.interactable = false;
				gameObject.SetActive(false);
				discovery.StopBroadcast();

				discovery.broadcastData = nameField.text;
				discovery.Initialize();
				discovery.StartAsServer();
				print("sending " + discovery.broadcastData);

				NetworkManager.singleton.StartHost();
				// transition
				/*CanvasGroup thisGroup = GetComponentInChildren<CanvasGroup>();
				thisGroup.interactable = false;
				thisGroup.alpha = 0f;*/
				lobbyCanvas.gameObject.SetActive(true);
			}
		);

		joinButton.onClick.AddListener (
			() => {
				joinButton.interactable = false;
				gameObject.SetActive(false);
				discovery.StopBroadcast();

				NetworkManager.singleton.networkAddress = activeHosts[selectedHostName];
				NetworkManager.singleton.StartClient();
				// transition
				/*CanvasGroup thisGroup = GetComponentInChildren<CanvasGroup>();
				thisGroup.interactable = false;
				thisGroup.alpha = 0f;*/
				lobbyCanvas.gameObject.SetActive(true);
			}
		);
	}

	public void ReceivedBroadcast(CustomNetworkDiscovery.HostInfo info) {
		if (!activeHosts.ContainsKey(info.hostName)) {
			print ("adding " + info.hostName);
			activeHosts.Add(info.hostName, info.hostIP);
			CreateHostButton (info.hostName);
		}
	}

	public void ClearedDict() {
		activeHosts.Clear ();
		joinButton.interactable = false;
		selectedHostName = "";
		selectedButton = null;
		hostNameText.text = "No hosts selected";
		BroadcastMessage ("ValidateName", nameField.text, SendMessageOptions.RequireReceiver);
		ClearGrid ();
	}

	void ClearGrid() {
		clearingGrid = true;
		while (activeHostsGrid.transform.childCount > 0)
			DestroyImmediate (activeHostsGrid.transform.GetChild (0).gameObject);
		clearingGrid = false;
	}

	void CreateHostButton(string hostName) {
		while (clearingGrid) ;

		GameObject newButton = (GameObject)Instantiate (activeHostButtonPrefab);
		newButton.transform.SetParent(activeHostsGrid.transform);
		newButton.GetComponent<RectTransform> ().localScale = Vector3.one;
											// apparently CanvasScaler modifies this
		newButton.GetComponentInChildren<Text> ().text = hostName;
		newButton.name = hostName;

		Button buttonComponent = newButton.GetComponent<Button> ();
		buttonComponent.onClick.AddListener (
			() => {
				if (selectedButton != null)
					selectedButton.interactable = true;
				selectedButton = buttonComponent;
				selectedHostName = hostName;
				buttonComponent.interactable = false;
				joinButton.interactable = (nameField.text != "");
				hostNameText.text = "Host: " + hostName;
			}
		);

		SortButtonsByName ();
	}

	void SortButtonsByName() {
		string[] keys = activeHosts.Keys.ToArray ();
		keys = keys.OrderBy (s => s, activeHosts.Comparer as IComparer<string>).ToArray();
		Button[] allButtons = activeHostsGrid.transform.GetComponentsInChildren<Button> ();
		foreach (Button button in allButtons) {
			button.gameObject.SetActive (false);
			button.transform.SetSiblingIndex (Array.BinarySearch (keys, button.gameObject.name));
		}
		foreach (Button button in allButtons) {
			button.gameObject.SetActive (true);
		}
	}

	void OnDisable() {
		discovery.StopBroadcast ();
	}
}
