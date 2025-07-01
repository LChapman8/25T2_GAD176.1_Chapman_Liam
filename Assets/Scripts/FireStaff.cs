using UnityEngine;

namespace SeaWizard.Weapons
{
    public class FireStaff : BaseStaff
    {
        private ParticleSystem fireVFX;
        private bool isCasting = false;

        public float manaCostPerSecond = 10f;

        protected override void Start()
        {
            base.Start();
            fireVFX = castPoint.GetComponentInChildren<ParticleSystem>(true);
            if (fireVFX != null) fireVFX.Stop();
            playerStats = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerStats>();
        }

        public override void StartCasting()
        {
            if (fireVFX != null && !isCasting && playerStats != null)
            {
                fireVFX.Play();
                isCasting = true;
                StartCoroutine(DrainManaWhileCasting());
            }
        }

        private System.Collections.IEnumerator DrainManaWhileCasting()
        {
            while (isCasting)
            {
                if (!playerStats.UseMana(manaCostPerSecond * Time.deltaTime))
                {
                    StopCasting();
                    yield break;
                }
                yield return null;
            }
        }

        public override void StopCasting()
        {
            if (fireVFX != null && isCasting)
            {
                fireVFX.Stop();
                isCasting = false;
            }
        }

        public override void CastSpell() { }
    }
}
