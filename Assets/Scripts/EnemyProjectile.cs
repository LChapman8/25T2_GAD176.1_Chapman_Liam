using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                Destroy(gameObject); // destroy projectile on hit
            }
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject); // destroy on hitting any solid object
        }
    }
}
