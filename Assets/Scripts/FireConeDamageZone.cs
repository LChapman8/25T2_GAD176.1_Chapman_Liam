using UnityEngine;
using SeaWizard.Enemy;

public class FireConeDamageZone : MonoBehaviour
{
    // sets dmg/s
    public float damagePerSecond = 40f;

    private void OnTriggerStay(Collider other)
    {
        // if enemy is in trigger zone, deal dmg/s
        MeleeEnemy enemy = other.GetComponent<MeleeEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}