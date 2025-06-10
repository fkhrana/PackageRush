using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainButton : MonoBehaviour
{
    public void PlayAgain()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.itemsCollected = 0;
            GameManager.instance.score = 0;
            GameManager.instance.door = null;
            GameManager.instance.isGameFinished = false;
        }
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBackgroundMusic();
        }
        SceneManager.LoadScene("SampleScene");
    }
}