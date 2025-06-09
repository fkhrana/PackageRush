using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour
{
    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager not found! Please add AudioManager to the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.isGameFinished = true;
        }
        AudioManager.instance.PlaySoundEffect("GameOver");
        AudioManager.instance.StopBackgroundMusic();
        SceneManager.LoadScene("GameOver");
    }
}