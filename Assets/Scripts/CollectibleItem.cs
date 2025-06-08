using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected || (GameManager.instance != null && GameManager.instance.isGameFinished)) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            Debug.Log($"Collecting item: {gameObject.name}, index: {itemIndex}");
            if (GameManager.instance != null)
            {
                GameManager.instance.CollectItem(itemIndex);
            }
            else
            {
                Debug.LogError("GameManager.instance is null!");
            }
            Destroy(gameObject);
        }
    }
}