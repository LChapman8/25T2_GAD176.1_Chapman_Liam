using UnityEngine;

namespace SeaWizard.Weapons
{
    public abstract class BaseStaff : MonoBehaviour
    {
        // variables for base staff stats 
        [Header("General Settings")]
        [SerializeField] protected float cooldownTime = 2f;
        [SerializeField] protected float projectileSpeed = 20f;
        [SerializeField] protected float damage = 10f;
        [SerializeField] protected float manaCost = 5f;
        // references for cast point and the projectile 

        [Header("References")]
        [SerializeField] protected Transform castPoint;
        [SerializeField] protected GameObject projectilePrefab;

        // variable for cooldown and a reference to players stats 
        protected bool isOnCooldown = false;
        protected PlayerStats playerStats;

        protected virtual void Start()
        {
            // Find the player to use mana later
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerStats = player.GetComponent<PlayerStats>();
        }

        // a function to say casting is available 
        protected bool CanCast()
        {
            return !isOnCooldown && playerStats != null && playerStats.UseMana(manaCost);
        }

        // a function to start the cooldown timer for spells 
        protected void StartCooldown()
        {
            isOnCooldown = true;
            Invoke(nameof(ResetCooldown), cooldownTime);
        }
        // function to reset the cooldown 
        private void ResetCooldown()
        {
            isOnCooldown = false;
        }

        public abstract void CastSpell();

        public virtual void StartCasting() { }
        public virtual void StopCasting() { }

        // optional getter if other systems need the damage value
        public float GetDamage() => damage;
    }
}