using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int itemsCollected = 0;
    public int requiredItems = 3;
    public GameObject door;

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
        if (scene.name == "SampleScene")
        {
            door = GameObject.FindWithTag("Door");
            itemsCollected = 0;
            if (door != null)
            {
                door.GetComponent<Collider2D>().isTrigger = false;
            }
        }
    }

    public void CollectItem()
    {
        itemsCollected++;
        Debug.Log("Items collected: " + itemsCollected);

        if (itemsCollected >= requiredItems && door != null)
        {
            UnlockDoor();
        }
    }

    private void UnlockDoor()
    {
        door.GetComponent<Collider2D>().isTrigger = true;
    }
}