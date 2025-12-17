using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainStage");
    }

    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
