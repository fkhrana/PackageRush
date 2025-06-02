using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("you touched my tralala");
        if (isCollected || (GameManager.instance != null && GameManager.instance.isGameFinished)) return;
        if (other.CompareTag("Player"))
        {
            isCollected = true;
            Debug.Log($"Collecting item: {gameObject.name}, instance ID: {gameObject.GetInstanceID()}");
            GameManager.instance.CollectItem();
            Destroy(gameObject);
        }
    }
}