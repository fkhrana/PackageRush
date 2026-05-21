using UnityEngine;
using TMPro;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private const string DefaultScoreText = "Score: 0";

    private void Start()
    {
        if (GameManager.instance != null)
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + GameManager.instance.score;
            }
            Debug.Log("Displayed score in GameWin: " + GameManager.instance.score);
        }
        else
        {
            if (scoreText != null)
            {
                scoreText.text = DefaultScoreText;
            }
            Debug.LogError("GameManager instance not found in GameWin!");
        }
    }
}