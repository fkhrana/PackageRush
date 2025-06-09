using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public void CloseTutorial()
    {
        SceneManager.LoadScene("SampleScene");
    }
}