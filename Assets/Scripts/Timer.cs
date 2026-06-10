using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TextMeshProUGUI timerText;
    [Header("Settings")]
    [SerializeField] private float remainingTime = 60f;
    private bool isGameOver = false;

    private const string GameOverSceneName = "GameOver";
    private const string GameOverSoundEffect = "GameOver";

    private void Start()
    {
        if (timerText != null)
        {
            timerText.color = Color.white;
        }
        isGameOver = false;
    }

    private void Update()
    {
        if (isGameOver || (GameManager.instance != null && GameManager.instance.isGameFinished)) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime <= 0)
        {
            remainingTime = 0;
            if (timerText != null)
            {
                timerText.color = Color.red;
            }
            isGameOver = true;
            if (GameManager.instance != null)
            {
                GameManager.instance.isGameFinished = true;
            }
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySoundEffect(GameOverSoundEffect);
                AudioManager.instance.StopBackgroundMusic(); 
            }
            else
            {
                Debug.LogWarning("AudioManager not found!");
            }
            ArduinoSerialInput.Instance?.SendGameOver();
            Debug.Log("Game Over: Time's up!");
            SceneManager.LoadScene(GameOverSceneName);
        }

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}