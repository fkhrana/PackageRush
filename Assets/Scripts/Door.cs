using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.instance != null && GameManager.instance.itemsCollected >= GameManager.instance.requiredItems)
        {
            GameManager.instance.isGameFinished = true;
            Debug.Log("Game Won! Player entered door.");
            SceneManager.LoadScene("GameWin");
        }
    }
}