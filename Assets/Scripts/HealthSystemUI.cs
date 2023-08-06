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
            else if (player2HealthSystem == null)
            {
                player2HealthSystem = player.GetComponent<HealthSystem>();
            }
        }
    }

    void Update()
    {
        if(player1HealthSystem != null&& player2HealthSystem != null)
        {
// Calculate the normalized value for the slider based on the players' health
        float normalizedValue = CalculateNormalizedHealth();

        // Set the Slider's value to the normalized value
        healthBarSlider.value = normalizedValue;
    }
        }
        

    private float CalculateNormalizedHealth()
    {
        
        // Calculate the total health of both players
        float totalHealth = player1HealthSystem.currentHealth + player2HealthSystem.currentHealth;

        // Calculate the normalized value based on the total health and max health
        // Divide by 20 (maxHealth * 2) to get a range from -1 to 1
        float normalizedValue = totalHealth / (maxHealth * 2);

        // The slider should move towards one direction based on the difference between the players' health
        // If player 1 has more health, the slider will move towards the right (1).
        // If player 2 has more health, the slider will move towards the left (-1).
        // If both players have the same health, the slider will be in the middle (0).

        return normalizedValue;
    }
}
