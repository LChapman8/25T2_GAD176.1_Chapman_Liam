using UnityEngine;
using SeaWizard.Enemy;

public class PoisonCloud : MonoBehaviour
{
    public float duration = 5f;
    public float damagePerSecond = 5f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}