using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.IO;

public class UIController : MonoBehaviour {
    public Button playButton;
    public Button quitButton;
    public Button settingsButton;
    public UIScaleSelector settingsPanel;

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        playButton.onClick.AddListener(Play);
        quitButton.onClick.AddListener(Quit);
        settingsButton.onClick.AddListener(LoadSettings);
    }

    void Play()
    {
        SceneManager.LoadScene(1);
    }

    void Quit()
    {
        Application.Quit();
    }

    void LoadSettings()
    {
        settingsPanel.gameObject.SetActive(true);
        settingsPanel.Activate();
    }
}
