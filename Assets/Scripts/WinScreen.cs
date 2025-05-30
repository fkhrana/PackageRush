using UnityEngine;
using TMPro;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        if (GameManager.instance != null)
        {
            scoreText.text = "Score: " + GameManager.instance.score;
            Debug.Log("Displayed score in GameWin: " + GameManager.instance.score);
        }
        else
        {
            scoreText.text = "Score: 0";
            Debug.LogError("GameManager instance not found in GameWin!");
        }
    }
}