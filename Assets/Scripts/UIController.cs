using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIController : MonoBehaviour {
    public Button playButton;    
    public Button quitButton;

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        playButton.onClick.AddListener(Play);
        quitButton.onClick.AddListener(Quit);
    }

    void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void Quit()
    {
        Application.Quit();
    }
}
