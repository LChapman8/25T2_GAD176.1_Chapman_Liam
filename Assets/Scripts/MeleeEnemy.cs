using UnityEngine;
using SeaWizard.Enemy;

public class MeleeEnemy : BaseEnemy
{
    // An assorted collection of variables across 4 catergories (detection, attack, wander and fleeing)
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

    // on start get reference to the players stats and set wander variables 
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
    // a function to update behaviours, for the melee enemy its to wander until the player enters detection range
    // then chase them down until theyre in attack range and attack them, when it hits 20HP, run away from the player
    protected override void UpdateBehavior()
    {
        if (!player) return;

        
        if (currentHealth <= fleeThreshold)
        {
            FleeFromPlayer();
            return;
        }

        
        playerDetected = CanSeePlayer();
        if (playerDetected)
        {
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

            return;
        }

       
        Wander();
    }
    // a function for wandering around when not in combat 
    void Wander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f)
        {
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
    // a function for fleeing from the player when at 20HP
    void FleeFromPlayer()
    {
        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        transform.position = Vector3.MoveTowards(transform.position, fleeTarget, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(fleeDirection);
    }
    // a function for detecting the player using raycasting 
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
    // a function for performing a melee attack when in attack range 
    void PerformMeleeAttack()
    {
        float attackDamage = Random.Range(5f, 11f);
        if (playerStats != null)
        {
            playerStats.TakeDamage(attackDamage);
            Debug.Log($"Player took {attackDamage} damage");
        }
    }
    // visualisations of my raycasting for testing purposes/debugging 
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
