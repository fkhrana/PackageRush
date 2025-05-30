using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected || (GameManager.instance != null && GameManager.instance.isGameFinished)) return;
        if (collision.CompareTag("Player"))
        {
            isCollected = true;
            Score score = FindFirstObjectByType<Score>();
            if (score != null)
            {
                score.AddScore(scoreValue);
            }
            else
            {
                Debug.LogError("Score component not found in scene! Ensure a GameObject with Score.cs exists.");
            }

            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(scoreValue);
            }
            else
            {
                Debug.LogError("GameManager instance not found!");
            }

            Destroy(gameObject);
        }
    }
}