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

    [Header("Wander")]
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;
    private float wanderTimer = 0f;
    private Vector3 wanderTarget;

    [Header("Fleeing")]
    public float fleeThreshold = 20f; // HP threshold to flee
    public float fleeDistance = 8f;

    private PlayerStats playerStats;
    private bool playerDetected = false;

    protected override void Start()
    {
        base.Start();

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }

        wanderTimer = wanderInterval;
        wanderTarget = transform.position;
    }

    protected override void UpdateBehavior()
    {
        if (!player) return;

        // If health low, flee
        if (currentHealth <= fleeThreshold)
        {
            FleeFromPlayer();
            return;
        }

        // Check for player
        playerDetected = CanSeePlayer();
        if (playerDetected)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > attackRange)
            {
                // Chase player
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                transform.LookAt(player);
            }
            else
            {
                // Attack if cooldown passed
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    lastAttackTime = Time.time;
                    PerformMeleeAttack();
                }
            }

            return;
        }

        // If no player in sight, wander
        Wander();
    }

    void Wander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f)
        {
            // Pick a new random spot around the enemy
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            Vector3 newTarget = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
            wanderTarget = newTarget;
            wanderTimer = wanderInterval;
        }

        transform.position = Vector3.MoveTowards(transform.position, wanderTarget, moveSpeed * 0.5f * Time.deltaTime);
        Vector3 direction = wanderTarget - transform.position;
        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void FleeFromPlayer()
    {
        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        transform.position = Vector3.MoveTowards(transform.position, fleeTarget, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(fleeDirection);
    }

    private bool CanSeePlayer()
    {
        if (!player) return false;

        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;
        directionToPlayer.Normalize();

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (distance > detectionRange || angle > fieldOfView * 0.5f)
            return false;

        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        if (Physics.Raycast(rayOrigin, directionToPlayer, out RaycastHit hit, detectionRange))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }

    void PerformMeleeAttack()
    {
        float attackDamage = Random.Range(5f, 11f);
        if (playerStats != null)
        {
            playerStats.TakeDamage(attackDamage);
            Debug.Log($"Player took {attackDamage} damage");
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
