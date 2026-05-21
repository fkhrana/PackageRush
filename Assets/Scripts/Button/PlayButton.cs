using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    private const string TutorialSceneName = "Tutorial";

    public void StartGame()
    {
        SceneManager.LoadScene(TutorialSceneName);
    }
}