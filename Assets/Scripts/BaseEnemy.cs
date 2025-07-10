using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for Slider reference if needed

namespace SeaWizard.Enemy
{
    public abstract class BaseEnemy : MonoBehaviour
    {
        [Header("Stats")]
        public float maxHealth = 100f;
        public float moveSpeed = 3f;

        protected Transform player;
        protected float currentHealth;

        [Header("Health Bar")]
        public GameObject healthBarPrefab;    // Assign this prefab in the Inspector
        private EnemyHealthBar healthBar;

        // sets current hp to max hp, finds the player and spawns health bar
        protected virtual void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            currentHealth = maxHealth;

            if (healthBarPrefab != null)
            {
                GameObject bar = Instantiate(healthBarPrefab);
                healthBar = bar.GetComponent<EnemyHealthBar>();
                healthBar.target = transform;             // Make health bar follow this enemy
                healthBar.SetHealth(currentHealth, maxHealth);
            }
        }

        // function to damage the enemies, update health bar
        public virtual void TakeDamage(float amount)
        {
            currentHealth -= amount;
            Debug.Log($"enemy now has {currentHealth} hp");

            if (healthBar != null)
                healthBar.SetHealth(currentHealth, maxHealth);

            if (currentHealth <= 0)
                Die();
        }

        protected virtual void Die()
        {
            Destroy(healthBar.gameObject);

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
