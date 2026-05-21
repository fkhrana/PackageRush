using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData; // Referensi ke Scriptable Object
    private bool isCollected = false;

    private const string PlayerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected || (GameManager.instance != null && GameManager.instance.isGameFinished)) return;

        if (other.CompareTag(PlayerTag))
        {
            if (itemData == null)
            {
                Debug.LogError($"ItemData is not assigned in {gameObject.name}!");
                return;
            }

            isCollected = true;
            Debug.Log($"Collecting item: {itemData.itemName}, index: {itemData.value}");
            if (GameManager.instance != null)
            {
                GameManager.instance.CollectItem(itemData);
            }
            else
            {
                Debug.LogError("GameManager.instance is null!");
            }
            Destroy(gameObject);
        }
    }
}