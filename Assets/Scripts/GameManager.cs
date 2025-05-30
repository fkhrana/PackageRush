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
            door = GameObject.FindWithTag("Door");
            if (door != null)
            {
                door.GetComponent<Collider2D>().isTrigger = false;
                Debug.Log("Door found and reset");
            }
            else
            {
                Debug.LogError("Door not found! Ensure the door has the 'Door' tag.");
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

    public void CollectItem()
    {
        if (isGameFinished) return;
        itemsCollected++;
        Debug.Log($"Item collected, total: {itemsCollected}/{requiredItems}");

        if (itemsCollected >= requiredItems && door != null)
        {
            UnlockDoor();
        }
    }

    public void AddScore(int points) // Dipanggil oleh PickUp.cs
    {
        if (isGameFinished) return;
        score += points;
        Debug.Log($"Score updated: {score}");
    }

    private void UnlockDoor()
    {
        door.GetComponent<Collider2D>().isTrigger = true;
        Debug.Log("Door unlocked");
    }
}