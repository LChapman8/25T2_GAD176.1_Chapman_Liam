using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // stats for health and mana 
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Mana")]
    public float maxMana = 50f;
    public float currentMana;
    public float manaRegenRate = 2f;

    [Header("Respawn")]
    public float respawnDelay = 2f;

    private CharacterController characterController;
    private Vector3 respawnPoint;
    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        characterController = GetComponent<CharacterController>();

        // Set initial respawn point to starting position
        respawnPoint = transform.position;
    }

    private void Update()
    {
        RegenerateMana();
    }

    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            currentMana = Mathf.Min(currentMana, maxMana);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }
        return false;
    }

    private void Die()
    {
        Debug.Log("Player died!");
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }
       

        // Disable controls if needed
        var grabController = GetComponent<PlayerGrabController>();
        if (grabController != null) grabController.enabled = false;

        // Start respawn
        Invoke(nameof(Respawn), respawnDelay);
    }

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

    // Optional: Call this from a checkpoint system to update the respawn point
    public void SetRespawnPoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
    }
}
