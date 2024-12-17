using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;  // Maksimal jon (3 ta)
    private int currentHealth; // Joriy jon
    public Text healthText;    // UIdagi jonni ko'rsatish uchun

    public ScoreManager scoreManager; // Score boshqaruvi

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth; // UIni yangilash
    }

    public void TakeDamage()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameOver();
        }

        UpdateHealthUI();
    }

    void GameOver()
    {
        scoreManager.ResetScore(); // Score-ni 0 ga tushirish
        Debug.Log("Game Over!");
    }
}
