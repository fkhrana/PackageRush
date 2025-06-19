using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private AudioData audioData; // Referensi ke Scriptable Object
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

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
                Debug.LogWarning("AudioData not assigned in AudioManager!");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (!backgroundMusic.isPlaying && backgroundMusic.clip != null)
        {
            backgroundMusic.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }

    public void PlaySoundEffect(string effectName)
    {
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