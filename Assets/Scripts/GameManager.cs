using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int itemsCollected = 0;
    public int requiredItems = 3;
    public int score = 0;
    public GameObject door;
    public bool isGameFinished = false;
    private CollectibleUI collectibleUI;
    private bool[] collectedItems = new bool[3];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager not found! Please add AudioManager to the scene.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        if (scene.name == "SampleScene")
        {
            Debug.Log("Resetting GameManager for SampleScene");
            itemsCollected = 0;
            score = 0;
            isGameFinished = false;
            collectedItems = new bool[3];
            door = GameObject.FindWithTag("Door");
            if (door != null)
            {
                door.GetComponent<Collider2D>().isTrigger = true;
                door.GetComponent<Door>().SetDoorSprite(false);
                Debug.Log("Door found and set as trigger with closed sprite");
            }
            else
            {
                Debug.LogError("Door not found! Ensure the door has the 'Door' tag.");
            }

            collectibleUI = FindFirstObjectByType<CollectibleUI>();
            if (collectibleUI != null)
            {
                collectibleUI.ResetIcons();
                Debug.Log("CollectibleUI found and icons reset");
            }
            else
            {
                Debug.LogWarning("CollectibleUI not found in scene!");
            }

            Score scoreComponent = FindFirstObjectByType<Score>();
            if (scoreComponent != null)
            {
                scoreComponent.ResetScore();
                Debug.Log("Score component reset");
            }
            else
            {
                Debug.LogWarning("Score component not found in scene!");
            }
        }
    }

    public void CollectItem(int itemIndex)
    {
        Debug.Log($"Collect Item called with index: {itemIndex}");
        if (isGameFinished || itemIndex < 0 || itemIndex >= collectedItems.Length) return;

        if (!collectedItems[itemIndex]) // Hanya collect jika belum dikumpul
        {
            collectedItems[itemIndex] = true;
            itemsCollected++;
            Debug.Log($"Item collected, total: {itemsCollected}/{requiredItems}");

            // Putar suara
            if (itemIndex == 0 || itemIndex == 1)
            {
                AudioManager.instance.PlaySoundEffect("BellCollect");
            }
            else if (itemIndex == 2)
            {
                AudioManager.instance.PlaySoundEffect("PhoneCollect");
            }

            if (collectibleUI != null)
            {
                collectibleUI.UpdateIcon(itemIndex);
            }

            if (itemsCollected >= requiredItems && door != null)
            {
                Debug.Log("Setting door to open sprite");
                door.GetComponent<Door>().SetDoorSprite(true);
                AudioManager.instance.PlaySoundEffect("DoorOpen");
            }
        }
        else
        {
            Debug.LogWarning($"Item with index {itemIndex} already collected!");
        }
    }

    public void AddScore(int points)
    {
        if (isGameFinished) return;
        score += points;
        Debug.Log($"Score updated: {score}");
    }
}