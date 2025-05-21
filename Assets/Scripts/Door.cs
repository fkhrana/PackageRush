using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.instance.itemsCollected >= GameManager.instance.requiredItems)
        {
            // Pemain masuk pintu, game selesai
            Debug.Log("Game Selesai!");
            // Pindah scene
            SceneManager.LoadScene("GameWin"); 
        }
    }
}