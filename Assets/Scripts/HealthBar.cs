using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private bool isGameOver = false; // Cegah transisi berulang

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        if (isGameOver) return; // Cegah perubahan health setelah game over
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);

        if (health <= 0)
        {
            isGameOver = true;
            if (GameManager.instance != null)
            {
                GameManager.instance.isGameFinished = true; // Tandai game selesai di GameManager
            }
            Debug.Log("Game Over: Health depleted!");
            SceneManager.LoadScene("GameOver");
        }
    }
}