using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // stats for health
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    // stats for mana
    [Header("Mana")]
    public float maxMana = 50f;
    public float currentMana;
    public float manaRegenRate = 2f;
    // variables for respawning 
    [Header("Respawn")]
    public float respawnDelay = 2f;

    private CharacterController characterController;
    private Vector3 respawnPoint;
    private Animator animator;
    private bool isDead = false;

    // on start set health and mana to max, get reference to the character controller  and set respawn point 
    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        characterController = GetComponent<CharacterController>();

        // Set the spawn point to starting position
        respawnPoint = transform.position;
    }
    // on update, regen mana 
    private void Update()
    {
        RegenerateMana();
    }
    // a function to regen mana based on time 
    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            currentMana = Mathf.Min(currentMana, maxMana);
        }
    }
    // a function for taking damage 
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    // a function for using mana 
    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }
        return false;
    }
    // a function that kills the player if they reach 0 currentHealth
    private void Die()
    {
        Debug.Log("Player died!");
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }
       

        // Disable controls so they cant move while dead
        var grabController = GetComponent<PlayerGrabController>();
        if (grabController != null) grabController.enabled = false;

        // starts respawning the player after a set delay 
        Invoke(nameof(Respawn), respawnDelay);
    }
    // a function that respawns the player and resets their stats and reenables their controls 
    private void Respawn()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        // Reset player position
        if (characterController != null)
        {
            characterController.enabled = false;
            transform.position = respawnPoint;
            characterController.enabled = true;
        }
        else
        {
            transform.position = respawnPoint;
        }

        // Re-enable controls
        var grabController = GetComponent<PlayerGrabController>();
        if (grabController != null) grabController.enabled = true;

        Debug.Log("Player respawned!");
    }
}
