using UnityEngine;

namespace SeaWizard.Weapons
{
    public class FireStaff : BaseStaff
    {
        private ParticleSystem fireVFX;
        private bool isCasting = false;

        // sets the cast point to my VFX for flame thrower in the scene 
        protected override void Start()
        {
            base.Start();
            fireVFX = castPoint.GetComponentInChildren<ParticleSystem>(true);
            if (fireVFX != null)
                fireVFX.Stop();
        }

        // function for starting casting by playing the particle effect
        public override void StartCasting()
        {
            if (CanCast() && fireVFX != null && !isCasting)
            {
                fireVFX.Play();
                isCasting = true;
            }
        }
        // function to stop playing the casting particle effect 
        public override void StopCasting()
        {
            if (fireVFX != null && isCasting)
            {
                fireVFX.Stop();
                isCasting = false;
            }
        }

        public override void CastSpell()
        {
            // fireStaff doesn't use burst casting so not needed.
        }
    }
}
