using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private bool isGameOver = false;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        if (isGameOver) return;
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
            SceneManager.LoadScene("GameOver");
        }
    }
}