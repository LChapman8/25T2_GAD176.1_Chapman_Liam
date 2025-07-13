using UnityEngine;

public class WaterKillBox : MonoBehaviour
{
    // A function so when the player hits the water (out of bounds area) they die immediantly
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(100f);
            }
        }
    }
}
