using UnityEngine;
using SeaWizard.Enemy;

public class MeleeEnemy : BaseEnemy
{
    [Header("Detection")]
    public float detectionRange = 10f;
    public float fieldOfView = 120f;

    [Header("Attack")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    private PlayerStats playerStats;
    private bool playerDetected = false;

    protected override void Start()
    {
        base.Start();
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }
    }

    protected override void UpdateBehavior()
    {
        if (!player) return;

        playerDetected = CanSeePlayer();
        if (!playerDetected) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            transform.LookAt(player);
        }
        else
        {
            if (Time.time - lastAttackTime > attackCooldown)
            {
                lastAttackTime = Time.time;
                PerformMeleeAttack();
            }
        }
    }

    private bool CanSeePlayer()
    {
        if (!player)
        {
            Debug.Log(" Player reference is missing.");
            return false;
        }

        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;
        directionToPlayer.Normalize();

        // 1. FOV angle check
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        Debug.Log($" Distance: {distance},  Angle: {angle}");

        if (distance > detectionRange)
        {
            Debug.Log(" Player is out of detection range.");
            return false;
        }

        if (angle > fieldOfView * 0.5f)
        {
            Debug.Log(" Player is outside field of view.");
            return false;
        }

        // 2. Raycast (line of sight)
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        Debug.DrawRay(rayOrigin, directionToPlayer * detectionRange, Color.red);

        if (Physics.Raycast(rayOrigin, directionToPlayer, out RaycastHit hit, detectionRange))
        {
            Debug.Log(" Raycast hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log(" Line of sight to player confirmed!");
                return true;
            }
            else
            {
                Debug.Log(" Line of sight blocked by: " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log(" Raycast didn't hit anything.");
        }

        return false;
    }

 


    void PerformMeleeAttack()
    {
        float attackDamage = Random.Range(5f, 11f);
        if (playerStats != null)
        {
            playerStats.TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 fovLine1 = Quaternion.Euler(0, fieldOfView * 0.5f, 0) * transform.forward;
        Vector3 fovLine2 = Quaternion.Euler(0, -fieldOfView * 0.5f, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1 * detectionRange);
        Gizmos.DrawRay(transform.position, fovLine2 * detectionRange);
    }
}
