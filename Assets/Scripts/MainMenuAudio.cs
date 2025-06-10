using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    [SerializeField] private GameObject audioManagerPrefab; // Drag prefab di Inspector

    private void Start()
    {
        Debug.Log("MainMenuAudio Start");
        if (AudioManager.instance == null)
        {
            Debug.Log("Instantiating AudioManager prefab");
            if (audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab);
            }
            else
            {
                Debug.LogError("AudioManager prefab not assigned!");
                return;
            }
        }
        // Tunggu frame berikutnya biar AudioManager diinisialisasi
        StartCoroutine(PlayBGM());
    }

    private System.Collections.IEnumerator PlayBGM()
    {
        yield return null; // Tunggu frame berikutnya
        if (AudioManager.instance != null)
        {
            Debug.Log("Playing BGM in MainMenu");
            AudioManager.instance.PlayBackgroundMusic();
        }
        else
        {
            Debug.LogError("AudioManager still null after instantiation!");
        }
    }
}