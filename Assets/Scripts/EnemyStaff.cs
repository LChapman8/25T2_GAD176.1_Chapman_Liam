using UnityEngine;
using SeaWizard.Enemy;

namespace SeaWizard.Weapons
{
    public class EnemyStaff : BaseStaff
    {
        private Transform player;
        private RangedEnemy rangedEnemy;
        

        protected override void Start()
        {
            base.Start();

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;

            // Instead of EnemyStats, get RangedEnemy component from parent
            rangedEnemy = GetComponentInParent<RangedEnemy>();
        }

        protected override bool CanCast()
        {
            // Check mana on RangedEnemy and cooldown
            return !isOnCooldown && rangedEnemy != null && rangedEnemy.currentMana >= manaCost;
        }

        public override void CastSpell()
        {
            if (!CanCast())
                return;

            if (player == null)
                return;

            Vector3 targetPos = player.position + Vector3.up * 1.5f; // aim higher on player
            Vector3 direction = (targetPos - castPoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, castPoint.position, Quaternion.LookRotation(direction));
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = direction * projectileSpeed;

            StartCooldown();
        }
    }
}

