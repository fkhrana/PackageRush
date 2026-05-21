using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    private const string SampleSceneName = "SampleScene";

    public void CloseTutorial()
    {
        SceneManager.LoadScene(SampleSceneName);
    }
}