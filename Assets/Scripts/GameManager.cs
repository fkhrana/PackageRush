using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton
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

    public void CollectItem()
    {
        itemsCollected++;
        Debug.Log("Items collected: " + itemsCollected);

        // Cek apakah semua item sudah dikumpulkan
        if (itemsCollected >= requiredItems)
        {
            UnlockDoor();
        }
    }

    private void UnlockDoor()
    {
        // Misalnya, nonaktifkan collider pintu atau ubah sprite pintu
        door.GetComponent<Collider2D>().isTrigger = true; // Aktifkan trigger pintu
        // Opsional: Ubah sprite pintu untuk menunjukkan pintu terbuka
        // door.GetComponent<SpriteRenderer>().sprite = openDoorSprite;
    }
}