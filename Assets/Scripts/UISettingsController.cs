using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsController : MonoBehaviour {
	private Slider gameDifficultySlider;
	private Slider musicalDifficultySlider;
	private Button acceptButton;
	private Button cancelButton;

	private void Awake() {
		gameDifficultySlider = transform.Find("GameDifficultySlider").GetComponent<Slider>();
		musicalDifficultySlider = transform.Find("MusicalDifficultySlider").GetComponent<Slider>();
		acceptButton = transform.Find("AcceptButton").GetComponent<Button>();
		cancelButton = transform.Find("CancelButton").GetComponent<Button>();
		acceptButton.onClick.AddListener(AcceptChanges);
		cancelButton.onClick.AddListener(CancelChanges);
		gameDifficultySlider.value = Settings.GameDifficulty;
		musicalDifficultySlider.value = Settings.MusicalDifficulty;
	}

	private void AcceptChanges() {
		Settings.GameDifficulty = gameDifficultySlider.value;
		Settings.MusicalDifficulty = musicalDifficultySlider.value;
		gameObject.SetActive(false);
	}

	private void CancelChanges() {
		gameObject.SetActive(false);
	}
}
