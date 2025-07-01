using UnityEngine;
using SeaWizard.Enemy;

public class PoisonCloud : MonoBehaviour
{
    // variables for dmg/s and duration
    public float duration = 5f;
    public float damagePerSecond = 5f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    // if an enemy is in the cloud they take damage/s
    private void OnTriggerStay(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}