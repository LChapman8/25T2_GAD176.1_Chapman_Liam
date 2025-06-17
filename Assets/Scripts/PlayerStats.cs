using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Mana")]
    public float maxMana = 50f;
    public float currentMana;
    public float manaRegenRate = 5f;

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Update()
    {
        RegenerateMana();
    }

    // function to regen mana based on amount per time passed
    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            currentMana = Mathf.Min(currentMana, maxMana); // Clamp to max
        }
    }

    //function for taking damage, triggering death if health hits 0
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // function for using mana on casting spells ( not set up yet) 
    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }
        return false;
    }

    // function for what happens when thep layer dies
    private void Die()
    {
        Debug.Log("Player died!");
        Destroy(gameObject);
    }
}

