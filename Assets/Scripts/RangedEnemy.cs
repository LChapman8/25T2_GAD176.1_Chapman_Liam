using UnityEngine;
using SeaWizard.Weapons;

namespace SeaWizard.Enemy
{
    public class RangedEnemy : BaseEnemy
    {
        // An assorted collection of variables across 4 catergories (mana, references, settings and wandering) 
        [Header("Mana Stats")]
        public float maxMana = 30f;
        public float currentMana;
        public float manaRegenRate = 1f;

        [Header("References")]
        public EnemyStaff staff;

        [Header("Settings")]
        public float attackRange = 8f;
        public float repositionDistance = 5f;
        public float attackCooldown = 2f;

        [Header("Wander")]
        public float wanderRadius = 5f;
        public float wanderInterval = 3f;

        private float lastAttackTime = -Mathf.Infinity;
        private Vector3 targetPosition;
        private bool isRepositioning = false;

        private float wanderTimer = 0f;
        private Vector3 wanderTarget;

        // on start, set mana to max, set staff to the equipt staff, set the target as the players position and set the wander varibles
        protected override void Start()
        {
            base.Start();

            currentMana = maxMana;

            if (staff == null)
                staff = GetComponentInChildren<EnemyStaff>();

            targetPosition = transform.position;

            wanderTimer = wanderInterval;
            wanderTarget = transform.position;
        }

        // a function that controls the ranged enemy behaviours which is wander until player enters detection range, then run to attack range and cast spells on cooldown
        // if attacked, run away from the player a set distance, then continue attacking.
        protected override void UpdateBehavior()
        {
            if (currentHealth <= 0)
                return;

            RegenerateMana();

            if (!CanSeePlayer())
            {
                // Wander if player not detected
                Wander();
                isRepositioning = false; // cancel repositioning if wandering
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (isRepositioning)
            {
                MoveToTarget();

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                    isRepositioning = false;
            }
            else
            {
                if (distanceToPlayer > attackRange)
                {
                    MoveTowards(player.position);
                }
                else
                {
                    FacePlayer();

                    if (Time.time >= lastAttackTime + attackCooldown)
                    {
                        if (CanCastSpell())
                        {
                            staff.CastSpell();
                            UseMana(staff.manaCost);
                            lastAttackTime = Time.time;
                        }
                    }
                }
            }
        }

        //a function for regening mana based on time 
        private void RegenerateMana()
        {
            if (currentMana < maxMana)
            {
                currentMana += manaRegenRate * Time.deltaTime;
                currentMana = Mathf.Min(currentMana, maxMana);
            }
        }
        // a function for allowing spells to be cast
        private bool CanCastSpell()
        {
            return currentMana >= staff.manaCost && !staff.isOnCooldown;
        }
        //a function for using mana
        private void UseMana(float amount)
        {
            currentMana -= amount;
        }
        // a function for moving towards towards a destination 
        private void MoveTowards(Vector3 destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            FaceDirection(destination - transform.position);
        }
        // a function for moving to the target 
        private void MoveToTarget()
        {
            MoveTowards(targetPosition);
        }
        // a function for making sure youre facing the player
        private void FacePlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            FaceDirection(direction);
        }
        // a function for changing the facing direction 
        private void FaceDirection(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        // a function for wandering when not in combat 
        private void Wander()
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
                FaceDirection(direction);
            }
        }
        // a function that uses raycasting to track the player and set a detection zone 
        private bool CanSeePlayer()
        {
            if (!player) return false;

            Vector3 directionToPlayer = player.position - transform.position;
            float distance = directionToPlayer.magnitude;
            directionToPlayer.Normalize();

            float detectionRange = 15f; 
            float fieldOfView = 120f;   

            if (distance > detectionRange)
                return false;

            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            if (angle > fieldOfView * 0.5f)
                return false;

            Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
            if (Physics.Raycast(rayOrigin, directionToPlayer, out RaycastHit hit, detectionRange))
            {
                return hit.collider.CompareTag("Player");
            }

            return false;
        }
        // a function for taking damage 
        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);

            if (currentHealth > 0)
                Reposition();
        }
        // a function for repositioning after taking damage 
        private void Reposition()
        {
            Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
            Vector3 randomOffset = Random.insideUnitSphere * repositionDistance;

            directionAwayFromPlayer.y = 0;
            randomOffset.y = 0;

            targetPosition = transform.position + directionAwayFromPlayer * repositionDistance + randomOffset;
            isRepositioning = true;
        }
    }
}
