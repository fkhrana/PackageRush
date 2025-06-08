using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite closedSprite; // Sprite pintu tertutup
    [SerializeField] private Sprite openSprite; // Sprite pintu terbuka
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite; // Set sprite default ke tertutup
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.instance != null && GameManager.instance.itemsCollected >= GameManager.instance.requiredItems)
        {
            GameManager.instance.isGameFinished = true;
            Debug.Log("Game Won! Player entered door.");
            SceneManager.LoadScene("GameWin");
        }
    }

    // Fungsi untuk mengganti sprite, dipanggil oleh GameManager
    public void SetDoorSprite(bool isOpen)
    {
        spriteRenderer.sprite = isOpen ? openSprite : closedSprite;
        Debug.Log($"Door sprite set to: {(isOpen ? "Open" : "Closed")}");
    }
}