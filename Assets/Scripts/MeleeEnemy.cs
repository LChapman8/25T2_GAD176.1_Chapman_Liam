using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeaWizard.Enemy;

public class MeleeEnemy : BaseEnemy
{
    // variables for attacking 
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;
    // reference to players stats 
    private PlayerStats playerStats;

    // get player stats so we can affect them
    protected override void Start()
    {
        base.Start();

        // Get the PlayerStats component from the player
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogWarning("PlayerStats component not found on player.");
            }
        }
    }
    // behaviour logic for melee enemy, chase player and attack them.
    protected override void UpdateBehavior()
    {
        if (!player) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // move toward the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            transform.LookAt(player);
        }
        else
        {
            // attack if cooldown time has passed
            if (Time.time - lastAttackTime > attackCooldown)
            {
                lastAttackTime = Time.time;
                PerformMeleeAttack();
            }
        }
    }
    // function for attacking the player 
    void PerformMeleeAttack()
    {
        // random damage from 5 to 10 inclusive
        float attackDamage = Random.Range(5f, 11f); 
        Debug.Log($"Melee enemy attacks with {attackDamage} damage!");

        if (playerStats != null)
        {
            playerStats.TakeDamage(attackDamage);
            Debug.Log($"Player took {attackDamage} damage.");
        }
    }
}
