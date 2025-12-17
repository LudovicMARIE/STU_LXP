using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("MainStage");
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("MainMenu");
    }
}