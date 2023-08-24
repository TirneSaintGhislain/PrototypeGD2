using UnityEngine;
using UnityEngine.UI;

public class HealthSystemUI : MonoBehaviour
{
    public Slider healthBarSlider;
    public int maxHealth = 20; // Updated to match the health range (-10 to 10)

    private HealthSystem player1HealthSystem;
    private HealthSystem player2HealthSystem;


    public void FindPlayerHealthSystem()
    {
        // Find the HealthSystem components attached to each player using NewPlayer object
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Assign the HealthSystem components to player1 and player2 based on availability
        foreach (GameObject player in players)
        {
            if (player1HealthSystem == null)
            {
                player1HealthSystem = player.GetComponent<HealthSystem>();
            }
            else if (player2HealthSystem == null && player1HealthSystem !=player.GetComponent<HealthSystem>())
            {
                player2HealthSystem = player.GetComponent<HealthSystem>();
            }
        }
    }

    void Update()
    {
        if (player1HealthSystem != null && player2HealthSystem != null)
        {
            // Calculate the normalized value for the slider based on the players' health
            float normalizedValue = CalculateNormalizedHealth();

            // Check if the slider's value reaches the minimum or maximum
            if (healthBarSlider.value <= healthBarSlider.minValue)
            {
                healthBarSlider.minValue = normalizedValue - 1;
               
            }
            else if (healthBarSlider.value >= healthBarSlider.maxValue)
            {
                healthBarSlider.maxValue = normalizedValue + 1;
            }

            // Set the Slider's value to the normalized value
            healthBarSlider.value = normalizedValue;
        }
    }



    private float CalculateNormalizedHealth()
    {
        // Calculate the total health of both players
        float totalHealth = player1HealthSystem._currentHealth - player2HealthSystem._currentHealth;

        float normalizedValue = totalHealth; /// (maxHealth*2);
        return normalizedValue;
    }

}
