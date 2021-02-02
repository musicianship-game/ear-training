using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public Button resumeButton;
    public Button mainMenuButton;
    public Button quitGameButton;
    public bool isPaused;
    
	void Start ()
    {
        isPaused = false;
        gameObject.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    public void PauseGame()
    {
        Debug.Log("Pausing the game");
        isPaused = true;
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming the game");
        isPaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void MainMenu()
    {
        Debug.Log("Going to the main menu");
        ResumeGame();
        SceneManager.LoadScene(0);
    }

    void QuitGame()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}
