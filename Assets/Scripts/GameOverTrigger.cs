using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}