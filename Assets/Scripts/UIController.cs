using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.IO;

public class UIController : MonoBehaviour {
    public Button playButton;
    public Button scaleSelectorButton;
    public Button settingsButton;
    public Button HowToPlayButton;
    public Button quitButton;
    public UIScaleSelector scaleSelectorPanel;
    public UISettingsController settingsPanel;

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        playButton.onClick.AddListener(Play);
        scaleSelectorButton.onClick.AddListener(ScaleSelection);
        quitButton.onClick.AddListener(Quit);
        settingsButton.onClick.AddListener(LoadSettings);
        HowToPlayButton.onClick.AddListener(HowToPlay);
    }

    void Play()
    {
        PlayerCloud.menu_time = Time.time;
        SceneManager.LoadScene(2);
    }

    void HowToPlay()
    {
        SceneManager.LoadScene(1);
    }

    void Quit()
    {
        Application.Quit();
    }

    void ScaleSelection()
    {
        scaleSelectorPanel.gameObject.SetActive(true);
        scaleSelectorPanel.Activate();
    }

    void LoadSettings()
    {
        settingsPanel.gameObject.SetActive(true);
        // settingsPanel.Activate();
    }
}
