using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger entered by: {collision.tag}");
        if (isCollected || (GameManager.instance != null && GameManager.instance.isGameFinished))
        {
            Debug.Log($"Pickup skipped: isCollected={isCollected}, isGameFinished={GameManager.instance?.isGameFinished}");
            return;
        }
        if (collision.CompareTag("Player"))
        {
            if (itemData == null)
            {
                Debug.LogError($"ItemData is not assigned in {gameObject.name}!");
                return;
            }
            Debug.Log($"Player collected item: {itemData.itemName}");
            isCollected = true;
            Score score = Score.Instance;
            if (score != null)
            {
                Debug.Log($"Calling Score.AddScore for {itemData.itemName}");
                score.AddScore(itemData);
            }
            else
            {
                Debug.LogError("Score component not found in scene!");
            }

            if (GameManager.instance != null && itemData.soundEffect != null)
            {
                Debug.Log($"Playing sound effect for {itemData.itemName}");
                AudioManager.instance.PlaySoundEffect(itemData.soundEffect);
            }
            else
            {
                Debug.LogError("GameManager or AudioManager instance not found, or sound effect is null!");
            }

            gameObject.SetActive(false);
        }
    }
}