using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;
    private SpriteRenderer spriteRenderer;

    private const string PlayerTag = "Player";
    private const string WinSceneName = "GameWin";
    private const string WinSoundEffect = "Win";

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = closedSprite;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on Door GameObject!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PlayerTag) && GameManager.instance != null && 
        GameManager.instance.itemsCollected >= GameManager.instance.requiredItems)
        {
            GameManager.instance.isGameFinished = true;
            AudioManager audioManager = AudioManager.instance;
            audioManager?.PlaySoundEffect(WinSoundEffect);
            audioManager?.StopBackgroundMusic();
            Debug.Log("Game Won! Player entered door.");
            SceneManager.LoadScene(WinSceneName);
        }
    }

    public void SetDoorSprite(bool isOpen)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Door sprite cannot be changed because SpriteRenderer is missing.");
            return;
        }

        spriteRenderer.sprite = isOpen ? openSprite : closedSprite;
        Debug.Log($"Door sprite set to: {(isOpen ? "Open" : "Closed")}");
    }
}