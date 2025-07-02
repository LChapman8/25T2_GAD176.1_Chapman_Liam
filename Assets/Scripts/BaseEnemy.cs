using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeaWizard.Enemy
{
    public abstract class BaseEnemy : MonoBehaviour
    {
        // the  variables for base enemy stats 
        [Header("Stats")]
        public float maxHealth = 100f;
        public float moveSpeed = 3f;

        protected Transform player;
        protected float currentHealth;

        // sets current hp to max hp, finds the player 
        protected virtual void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            currentHealth = maxHealth;
        }
        // function to damage the enemies, need to rig to staff dmg
        public virtual void TakeDamage(float amount)
        {
            currentHealth -= amount;
            Debug.Log($"enemy now has {currentHealth} hp");

            if (currentHealth <= 0)
                Die();
        }

        protected virtual void Die()
        {
            // set up death animation / vfx / link to respawn system.
            Destroy(gameObject);
        }

        // all my child enemies must implement this function to work
        protected abstract void UpdateBehavior();

        private void Update()
        {
            UpdateBehavior();
        }
    }
}
