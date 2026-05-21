using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private bool isGameOver = false;

    private const string GameOverSceneName = "GameOver";

    public void SetMaxHealth(int health)
    {
        if (slider == null || fill == null || gradient == null)
        {
            Debug.LogWarning("HealthBar is missing slider, fill, or gradient references.");
            return;
        }

        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        if (isGameOver) return;

        if (slider == null || fill == null || gradient == null)
        {
            Debug.LogWarning("HealthBar is missing slider, fill, or gradient references.");
            return;
        }

        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);

        if (health <= 0)
        {
            isGameOver = true;
            if (GameManager.instance != null)
            {
                GameManager.instance.isGameFinished = true;
            }
            Debug.Log("Game Over: Health depleted!");
            SceneManager.LoadScene(GameOverSceneName);
        }
    }
}