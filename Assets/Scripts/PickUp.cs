using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    private bool isCollected = false;

    private const string PlayerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger entered by: {collision.tag}");
        if (isCollected || (GameManager.instance != null && GameManager.instance.isGameFinished))
        {
            Debug.Log($"Pickup skipped: isCollected={isCollected}, isGameFinished={GameManager.instance?.isGameFinished}");
            return;
        }
        if (collision.CompareTag(PlayerTag))
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

            if (itemData.soundEffect != null && AudioManager.instance != null)
            {
                Debug.Log($"Playing sound effect for {itemData.itemName}");
                AudioManager.instance.PlaySoundEffect(itemData.soundEffect);
            }

            gameObject.SetActive(false);
        }
    }
}