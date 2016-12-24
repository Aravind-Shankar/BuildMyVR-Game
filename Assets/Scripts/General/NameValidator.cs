using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(InputField))]
public class NameValidator : MonoBehaviour {

	public Text errorText;
	public Button hostButton;
	public Button joinButton;

	private InputField field;

	void Awake() {
		field = GetComponent<InputField> ();
	}

	void OnEnable() {
		ValidateName (field.text);
	}

	public void ValidateName(string enteredName) {
		if (enteredName == "") {
			errorText.text = "Name cannot be empty.";
		}
		else {
			errorText.text = "";
			field.text = enteredName.ToUpper ().Trim();
			if (GameFinder.instance.activeHosts.ContainsKey (field.text))
				errorText.text = "Name already taken by an active host.";
		}

		hostButton.interactable = (errorText.text == "");
		joinButton.interactable = (enteredName != "") && (GameFinder.instance.selectedHostName != "");


	}

}
