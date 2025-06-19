using UnityEngine;

[CreateAssetMenu(fileName = "New AudioData", menuName = "Audio/AudioData")]
public class AudioData : ScriptableObject
{
    [System.Serializable]
    public struct AudioEntry
    {
        public string effectName;
        public AudioClip clip;
    }

    public AudioEntry[] audioEntries;
    public AudioClip backgroundMusicClip; // Untuk background music
}