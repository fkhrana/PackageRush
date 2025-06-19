using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    [SerializeField] private GameObject audioManagerPrefab;

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
        StartCoroutine(PlayBGM());
    }

    private System.Collections.IEnumerator PlayBGM()
    {
        yield return null;
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