using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private AudioData audioData; // Referensi ke Scriptable Object
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private const string AudioDataWarning = "AudioData not assigned in AudioManager!";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioClips.Clear();

            // Inisialisasi AudioSource kalau null
            if (backgroundMusic == null)
            {
                backgroundMusic = gameObject.AddComponent<AudioSource>();
                backgroundMusic.loop = true;
            }
            if (soundEffect == null)
            {
                soundEffect = gameObject.AddComponent<AudioSource>();
            }

            // Inisialisasi dictionary dari AudioData
            if (audioData != null)
            {
                foreach (var entry in audioData.audioEntries)
                {
                    if (!string.IsNullOrEmpty(entry.effectName) && entry.clip != null)
                    {
                        audioClips[entry.effectName] = entry.clip;
                    }
                }
                if (audioData.backgroundMusicClip != null)
                {
                    backgroundMusic.clip = audioData.backgroundMusicClip;
                }
            }
            else
            {
                Debug.LogWarning(AudioDataWarning);
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying && backgroundMusic.clip != null)
        {
            backgroundMusic.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
    }

    public void PlaySoundEffect(string effectName)
    {
        if (soundEffect == null)
        {
            Debug.LogWarning("Sound effect AudioSource is not assigned!");
            return;
        }

        if (audioClips.TryGetValue(effectName, out AudioClip clip))
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Audio clip for {effectName} not found!");
        }
    }
}