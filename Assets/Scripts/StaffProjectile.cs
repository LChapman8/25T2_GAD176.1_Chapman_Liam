using UnityEngine;
using SeaWizard.Enemy;

public class StaffProjectile : MonoBehaviour
{
    private float damage;
    public bool appliesSlow = false;
    public float slowFactor = 0.5f;
    public float slowDuration = 2f;

    public void SetDamage(float dmg) => damage = dmg;

    private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.collider.GetComponent<BaseEnemy>();
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
