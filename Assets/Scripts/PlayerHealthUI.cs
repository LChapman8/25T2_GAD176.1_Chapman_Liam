using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    // updates the health bar UI visual based on the players health
    public Slider healthSlider; 
    public PlayerStats playerStats; 

    void Update()
    {
        healthSlider.value = playerStats.currentHealth / playerStats.maxHealth;
    }
}

