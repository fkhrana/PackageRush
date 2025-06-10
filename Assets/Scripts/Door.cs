using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite;
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager not found! Please add AudioManager to the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.instance != null && 
        GameManager.instance.itemsCollected >= GameManager.instance.requiredItems)
        {
            GameManager.instance.isGameFinished = true;
            AudioManager.instance.PlaySoundEffect("Win");
            AudioManager.instance.StopBackgroundMusic();
            Debug.Log("Game Won! Player entered door.");
            SceneManager.LoadScene("GameWin");
        }
    }

    public void SetDoorSprite(bool isOpen)
    {
        spriteRenderer.sprite = isOpen ? openSprite : closedSprite;
        Debug.Log($"Door sprite set to: {(isOpen ? "Open" : "Closed")}");
    }
}