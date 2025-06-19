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
    [SerializeField] private GameObject audioManagerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager singleton initialized");
        }
        else
        {
            Debug.LogWarning("Duplicate GameManager instance found, destroying this one");
            Destroy(gameObject);
        }

        EnsureAudioManagerExists();
    }

    private void Start()
    {
        Score scoreComponent = Score.Instance;
        if (scoreComponent != null)
        {
            scoreComponent.OnScoreChanged += UpdateScore;
            Debug.Log("Subscribed to Score.OnScoreChanged");
        }
        else
        {
            Debug.LogError("Score component not found when subscribing!");
        }
    }

    private void EnsureAudioManagerExists()
    {
        if (AudioManager.instance == null)
        {
            Debug.LogWarning("AudioManager not found! Creating from prefab.");
            if (audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab);
            }
            else
            {
                Debug.LogError("AudioManager prefab not assigned in GameManager!");
            }
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

            Score scoreComponent = Score.Instance;
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

    private void UpdateScore(int newScore)
    {
        score = newScore;
        Debug.Log($"GameManager score updated: {score}");
    }

    public void CollectItem(ItemData itemData)
    {
        if (isGameFinished || itemData == null || itemData.value < 0 || itemData.value >= collectedItems.Length) return;

        if (!collectedItems[itemData.value])
        {
            collectedItems[itemData.value] = true;
            itemsCollected++;
            Debug.Log($"Item collected: {itemData.itemName}, total: {itemsCollected}/{requiredItems}");

            if (itemData.soundEffect != null && AudioManager.instance != null)
            {
                AudioManager.instance.PlaySoundEffect(itemData.soundEffect);
            }

            if (collectibleUI != null)
            {
                collectibleUI.UpdateIcon(itemData.value);
            }

            if (itemsCollected >= requiredItems && door != null)
            {
                Debug.Log("Setting door to open sprite");
                door.GetComponent<Door>().SetDoorSprite(true);
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlaySoundEffect("DoorOpen");
                }
            }
        }
        else
        {
            Debug.LogWarning($"Item {itemData.itemName} already collected!");
        }
    }
}