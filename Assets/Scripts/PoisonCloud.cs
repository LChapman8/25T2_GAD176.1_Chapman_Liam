using UnityEngine;
using SeaWizard.Enemy;

public class PoisonCloud : MonoBehaviour
{
    // variables for dmg/s and duration
    public float duration = 5f;
    public float damagePerSecond = 30f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    // a function for dealing dmg/s when inside the cloud 
    private void OnTriggerStay(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}