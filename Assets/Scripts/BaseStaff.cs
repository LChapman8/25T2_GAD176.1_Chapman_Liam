using System.Collections;
using System.Collections.Generic;
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
        private PlayerGrabController grabStaffController;

        private void Start()
        {
            grabStaffController = FindAnyObjectByType<PlayerGrabController>();
            if (grabStaffController == null)
            {
                Debug.LogWarning("PlayerGrabController not found in scene.");
            }
        }

        protected bool CanCast()
        {
            return !isOnCooldown;
        }

        protected void StartCooldown()
        {
            isOnCooldown = true;
            Invoke(nameof(ResetCooldown), cooldownTime);
        }

        private void ResetCooldown()
        {
            isOnCooldown= false;
        }

        public abstract void CastSpell();
    }
}
