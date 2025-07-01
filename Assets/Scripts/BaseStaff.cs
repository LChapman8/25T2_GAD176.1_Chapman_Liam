using UnityEngine;

namespace SeaWizard.Weapons
{
    public abstract class BaseStaff : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] protected float cooldownTime = 2f;
        [SerializeField] protected float projectileSpeed = 20f;
        [SerializeField] protected float damage = 10f;
        [SerializeField] protected float manaCost = 5f;

        [Header("References")]
        [SerializeField] protected Transform castPoint;
        [SerializeField] protected GameObject projectilePrefab;

        protected bool isOnCooldown = false;
        protected PlayerStats playerStats;

        protected virtual void Start()
        {
            // Find the player to use mana later
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerStats = player.GetComponent<PlayerStats>();
        }

        protected bool CanCast()
        {
            return !isOnCooldown && playerStats != null && playerStats.UseMana(manaCost);
        }

        protected void StartCooldown()
        {
            isOnCooldown = true;
            Invoke(nameof(ResetCooldown), cooldownTime);
        }

        private void ResetCooldown()
        {
            isOnCooldown = false;
        }

        public abstract void CastSpell();

        public virtual void StartCasting() { }
        public virtual void StopCasting() { }

        // Optional getter if other systems need the damage value
        public float GetDamage() => damage;
    }
}