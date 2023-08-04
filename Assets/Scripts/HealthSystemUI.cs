using UnityEngine;
using UnityEngine.UI;

public class HealthSystemUI : MonoBehaviour
{
    public Slider sharedHealthSlider;

    public int maxHealth = 100;
    private float currentHealthPercentage = 0.5f; // Starting health percentage (50%)

    public float healthGainAmount = 0.1f; // Amount of health gained on each action
    public float healthLossAmount = 0.2f; // Amount of health lost on each action

    void Start()
    {
        SetSharedHealthUI();
    }

    // Method to handle player actions that gain health
    public void GainHealth()
    {
        currentHealthPercentage = Mathf.Clamp(currentHealthPercentage + healthGainAmount, 0f, 1f);
        SetSharedHealthUI();
    }

    // Method to handle player actions that lose health
    public void LoseHealth()
    {
        currentHealthPercentage = Mathf.Clamp(currentHealthPercentage - healthLossAmount, 0f, 1f);
        SetSharedHealthUI();
    }

    void SetSharedHealthUI()
    {
        sharedHealthSlider.value = currentHealthPercentage * maxHealth;
    }
}
