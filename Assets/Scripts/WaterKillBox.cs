using UnityEngine;

public class WaterKillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(100f); // Or stats.TakeDamage(stats.maxHealth);
            }
        }
    }
}
