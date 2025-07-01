using UnityEngine;
using SeaWizard.Enemy;

public class FireConeDamageZone : MonoBehaviour
{
    public float damagePerSecond = 20f;

    private void OnTriggerStay(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}