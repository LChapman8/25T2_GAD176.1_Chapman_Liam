using UnityEngine;
using SeaWizard.Enemy;

public class StaffProjectile : MonoBehaviour
{
    // variables for damage and slowing effect 
    private float damage;
    public bool appliesSlow = false;
    public float slowFactor = 0.5f;
    public float slowDuration = 2f;

    // function for setting damage 
    public void SetDamage(float dmg) => damage = dmg;
    // the logic for if the projectile hits and theyre an enemy, take dmg and apply the slow and then destroy the game object 
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            if (appliesSlow)
            {
                SlowEffect slow = enemy.GetComponent<SlowEffect>();
                if (slow == null) slow = enemy.gameObject.AddComponent<SlowEffect>();
                slow.ApplySlow(slowFactor, slowDuration);
            }
        }

        Destroy(gameObject);
    }

}
