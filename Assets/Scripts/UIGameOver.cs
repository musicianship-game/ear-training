using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameOver : MonoBehaviour {
    //public TextMeshProUGUI timeElapsed;
    public TextMeshProUGUI scale;
    public TextMeshProUGUI score;
    public TextMeshProUGUI defeated;
    public TextMeshProUGUI accuracy;
    public TextMeshProUGUI misspellings;

	// Use this for initialization
	void Start () {
        //timeElapsed = transform.Find("TimeElapsed").GetComponent<TextMeshProUGUI>();
        scale = transform.Find("ScaleText").GetComponent<TextMeshProUGUI>();
        score = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        defeated = transform.Find("EnemiesDefeatedText").GetComponent<TextMeshProUGUI>();
        accuracy = transform.Find("AccuracyText").GetComponent<TextMeshProUGUI>();
        misspellings = transform.Find("MisspelledNotesText").GetComponent<TextMeshProUGUI>();

        scale.text = Scale.Name;
        //timeElapsed.text = "Duration of the game: " + (Time.time - PlayerCloud.menu_time);
        score.text = "Score: " + PlayerCloud.score;
        defeated.text = "Defeated enemies: " + PlayerCloud.enemies_defeated;
        accuracy.text = "Accuracy: " + Mathf.Round(PlayerCloud.GetAccuracy() * 100f) + "%";
        misspellings.text = "Misspellings: " + PlayerCloud.misspellings;
    }
}
