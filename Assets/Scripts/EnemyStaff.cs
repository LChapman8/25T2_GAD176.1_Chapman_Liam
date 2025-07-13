using UnityEngine;
using SeaWizard.Enemy;

namespace SeaWizard.Weapons
{
    public class EnemyStaff : BaseStaff
    {
        // transform for player and the staffs user
        private Transform player;
        private RangedEnemy rangedEnemy;
        
        // on start, find the players location and then get the stats from ranged enemy
        protected override void Start()
        {
            base.Start();

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;

            // Instead of EnemyStats, get RangedEnemy component from parent
            rangedEnemy = GetComponentInParent<RangedEnemy>();
        }
        // a function to check if the ranged enemy is able to cast
        protected override bool CanCast()
        {
            return !isOnCooldown && rangedEnemy != null && rangedEnemy.currentMana >= manaCost;
        }
        // a function for casting at the players position
        public override void CastSpell()
        {
            if (!CanCast())
                return;

            if (player == null)
                return;

            Vector3 targetPos = player.position + Vector3.up * 1.5f;
            Vector3 direction = (targetPos - castPoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, castPoint.position, Quaternion.LookRotation(direction));
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = direction * projectileSpeed;

            StartCooldown();
        }
    }
}

