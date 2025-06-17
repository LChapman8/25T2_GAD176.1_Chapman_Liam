using UnityEngine;

namespace SeaWizard.Weapons
{
    public abstract class BaseStaff : MonoBehaviour
    {
        [SerializeField] protected float cooldownTime = 2f;
        [SerializeField] protected Transform castPoint;
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected float projectileSpeed = 20f;

        protected bool isOnCooldown = false;

        protected virtual void Start()
        {
            // optional for my derived classes
        }

        // function for casting
        protected bool CanCast()
        {
            return !isOnCooldown;
        }

        protected void StartCooldown()
        {
            isOnCooldown = true;
            Invoke(nameof(ResetCooldown), cooldownTime);
        }

        //function for resetting the cooldown timer
        private void ResetCooldown()
        {
            isOnCooldown = false;
        }

        public abstract void CastSpell();

        // optional overrides for my continuous effect spells
        public virtual void StartCasting() { }
        public virtual void StopCasting() { }
    }
}
