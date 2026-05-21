using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const string GameOverSceneName = "GameOver";
    private const string GameOverSoundEffect = "GameOver";

    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager not found! Please add AudioManager to the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
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
        AudioManager audioManager = AudioManager.instance;
        audioManager?.PlaySoundEffect(GameOverSoundEffect);
        audioManager?.StopBackgroundMusic();
        SceneManager.LoadScene(GameOverSceneName);
    }
}