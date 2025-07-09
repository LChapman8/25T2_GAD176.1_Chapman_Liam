using UnityEngine;
using SeaWizard.Enemy;
using SeaWizard.Weapons;

public class FireConeDamageZone : MonoBehaviour
{
    // sets dmg/s
    public float damagePerSecond = 30f;

    private void OnTriggerStay(Collider other)
    {
        // if enemy is in trigger zone, deal dmg/s
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
           enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}