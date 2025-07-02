using UnityEngine;

namespace SeaWizard.Weapons
{
    public class FireStaff : BaseStaff
    {
        // reference for the flamer thrower vfx
        private ParticleSystem fireVFX;
        // variable for if youre casting and the mana cost per second to cast
        private bool isCasting = false;

        public float manaCostPerSecond = 10f;

        // on start set the VFX to the vfx assigned as the child of castpoint on the staff
        protected override void Start()
        {
            base.Start();
            fireVFX = castPoint.GetComponentInChildren<ParticleSystem>(true);
            if (fireVFX != null) fireVFX.Stop();
            playerStats = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerStats>();
        }
        // if the criteria is met, start casting the vfx/spell 
        public override void StartCasting()
        {
            if (fireVFX != null && !isCasting && playerStats != null)
            {
                fireVFX.Play();
                isCasting = true;
                StartCoroutine(DrainManaWhileCasting());
            }
        }
        // while casting is active, actively drain specified mana amount 
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
        //when the player stops casting, end the vfx 
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
