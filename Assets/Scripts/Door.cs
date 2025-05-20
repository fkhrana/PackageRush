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
            // Opsional: Pindah ke scene lain atau tampilkan UI kemenangan
            SceneManager.LoadScene("GameWin"); // Ganti dengan nama scene yang sesuai
        }
    }
}