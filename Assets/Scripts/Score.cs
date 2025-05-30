using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    private void Start()
    {
        ResetScore();
    }

    private void Update()
    {
        SetScoreToTMP();
    }

    public void AddScore(int value)
    {
        if (GameManager.instance != null && GameManager.instance.isGameFinished) return;
        score += value;
        Debug.Log("Score: " + score);
    }

    public void ResetScore()
    {
        score = 0;
        SetScoreToTMP();
    }

    private void SetScoreToTMP()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}