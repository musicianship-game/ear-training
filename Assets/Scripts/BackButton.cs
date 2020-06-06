using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void Back()
    {
        PlayerCloud.Restart();
        SceneManager.LoadScene(0);
    }
}