using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    [SerializeField] private GameObject audioManagerPrefab;

    private const string AudioManagerPrefabMissingMessage = "AudioManager prefab not assigned!";

    private void Start()
    {
        EnsureAudioManagerExists();
        StartCoroutine(PlayBGM());
    }

    private void EnsureAudioManagerExists()
    {
        if (AudioManager.instance != null)
        {
            return;
        }

        Debug.Log("Instantiating AudioManager prefab");
        if (audioManagerPrefab != null)
        {
            Instantiate(audioManagerPrefab);
            return;
        }

        Debug.LogError(AudioManagerPrefabMissingMessage);
    }

    private System.Collections.IEnumerator PlayBGM()
    {
        yield return null;
        if (AudioManager.instance != null)
        {
            Debug.Log("Playing BGM in MainMenu");
            AudioManager.instance.PlayBackgroundMusic();
            yield break;
        }

        Debug.LogError("AudioManager still null after instantiation!");
    }
}