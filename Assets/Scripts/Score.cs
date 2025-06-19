using UnityEngine;
using TMPro;
using System;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Score singleton initialized");
        }
        else
        {
            Debug.LogWarning("Duplicate Score instance found, destroying this one");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetScore();
    }

    public void AddScore(ItemData itemData)
    {
        if (GameManager.instance != null && GameManager.instance.isGameFinished)
        {
            Debug.Log("AddScore skipped: Game is finished.");
            return;
        }
        if (itemData == null)
        {
            Debug.LogError("ItemData is null in AddScore!");
            return;
        }
        if (itemData.isKeyItem)
        {
            Debug.Log($"Item {itemData.itemName} is a key item, no score added.");
            return;
        }
        score += itemData.value;
        SetScoreToTMP();
        OnScoreChanged?.Invoke(score);
        Debug.Log($"Score updated: {score} (+{itemData.value} from {itemData.itemName})");
    }

    public void ResetScore()
    {
        score = 0;
        SetScoreToTMP();
        OnScoreChanged?.Invoke(score);
        Debug.Log("Score reset to 0");
    }

    private void SetScoreToTMP()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
            Debug.Log($"Score text updated to: {score}");
        }
        else
        {
            Debug.LogError("Score TextMeshProUGUI is not assigned in Inspector!");
        }
    }
}