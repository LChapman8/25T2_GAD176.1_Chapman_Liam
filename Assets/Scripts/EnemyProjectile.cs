using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // sets the projectiles damage 
    public float damage = 10f;
    // if the projectile collides with the players collider, deal the damage and destroy the projectile
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                Destroy(gameObject); 
            }
        }
        // destroy projectile if it collides with any other solid object
        else if (!other.isTrigger)
        {
            Destroy(gameObject); 
        }
    }
}
