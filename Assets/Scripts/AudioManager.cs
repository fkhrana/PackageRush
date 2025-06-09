using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private AudioClip dogHit, policeHit, waterSplash, starCollect, bellCollect, phoneCollect, gameOver, win, doorOpen;

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

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (!backgroundMusic.isPlaying)
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
        AudioClip clip = null;
        switch (effectName)
        {
            case "DogHit": clip = dogHit; break;
            case "PoliceHit": clip = policeHit; break;
            case "WaterSplash": clip = waterSplash; break;
            case "StarCollect": clip = starCollect; break;
            case "BellCollect": clip = bellCollect; break;
            case "PhoneCollect": clip = phoneCollect; break;
            case "GameOver": clip = gameOver; break;
            case "Win": clip = win; break;
            case "DoorOpen": clip = doorOpen; break;
        }
        if (clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Audio clip for {effectName} not found!");
        }
    }
}