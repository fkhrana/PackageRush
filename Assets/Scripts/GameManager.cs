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
    private Score subscribedScore;
    private Door doorController;
    private Collider2D doorCollider;

    private const string SampleSceneName = "SampleScene";
    private const string DoorOpenSoundEffect = "DoorOpen";

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
            return;
        }

        EnsureAudioManagerExists();
    }

    private void Start()
    {
        SubscribeToScore();
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

        if (subscribedScore != null)
        {
            subscribedScore.OnScoreChanged -= UpdateScore;
            subscribedScore = null;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        if (scene.name != SampleSceneName)
        {
            return;
        }

        Debug.Log("Resetting GameManager for SampleScene");
        ResetGameplayState();
        CacheDoor();
        ResetCollectibleUI();
        ResetScoreState();
    }

    private void SubscribeToScore()
    {
        Score scoreComponent = Score.Instance;
        if (scoreComponent == null)
        {
            Debug.LogError("Score component not found when subscribing!");
            return;
        }

        if (subscribedScore == scoreComponent)
        {
            return;
        }

        if (subscribedScore != null)
        {
            subscribedScore.OnScoreChanged -= UpdateScore;
        }

        subscribedScore = scoreComponent;
        subscribedScore.OnScoreChanged += UpdateScore;
        Debug.Log("Subscribed to Score.OnScoreChanged");
    }

    private void ResetGameplayState()
    {
        itemsCollected = 0;
        score = 0;
        isGameFinished = false;
        collectedItems = new bool[Mathf.Max(requiredItems, 1)];
    }

    private void CacheDoor()
    {
        door = GameObject.FindWithTag("Door");
        doorController = null;
        doorCollider = null;

        if (door == null)
        {
            Debug.LogError("Door not found! Ensure the door has the 'Door' tag.");
            return;
        }

        if (!door.TryGetComponent(out doorCollider))
        {
            Debug.LogError("Door collider not found on the door GameObject!");
        }
        else
        {
            doorCollider.isTrigger = true;
        }

        if (!door.TryGetComponent(out doorController))
        {
            Debug.LogError("Door component not found on the door GameObject!");
            return;
        }

        doorController.SetDoorSprite(false);
        Debug.Log("Door found and set as trigger with closed sprite");
    }

    private void ResetCollectibleUI()
    {
        collectibleUI = FindFirstObjectByType<CollectibleUI>();
        if (collectibleUI == null)
        {
            Debug.LogWarning("CollectibleUI not found in scene!");
            return;
        }

        collectibleUI.ResetIcons();
        Debug.Log("CollectibleUI found and icons reset");
    }

    private void ResetScoreState()
    {
        SubscribeToScore();

        if (subscribedScore == null)
        {
            Debug.LogWarning("Score component not found in scene!");
            return;
        }

        subscribedScore.ResetScore();
        Debug.Log("Score component reset");
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

            AudioManager audioManager = AudioManager.instance;
            if (audioManager != null && !string.IsNullOrEmpty(itemData.soundEffect))
            {
                audioManager.PlaySoundEffect(itemData.soundEffect);
            }

            if (collectibleUI != null)
            {
                collectibleUI.UpdateIcon(itemData.value);
            }

            if (itemsCollected >= requiredItems && doorController != null)
            {
                Debug.Log("Setting door to open sprite");
                doorController.SetDoorSprite(true);
                if (audioManager != null)
                {
                    audioManager.PlaySoundEffect(DoorOpenSoundEffect);
                }
            }
        }
        else
        {
            Debug.LogWarning($"Item {itemData.itemName} already collected!");
        }
    }
}