using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Slider healthSlider; // Assign in Inspector
    public PlayerStats playerStats; // Assign in Inspector

    void Update()
    {
        healthSlider.value = playerStats.currentHealth / playerStats.maxHealth;
    }
}

