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
            Debug.Log("GameManager reset for Play Again");
        }
        SceneManager.LoadScene("SampleScene");
    }
}