using UnityEngine;

namespace SeaWizard.Weapons
{
    public class PoisonStaff : BaseStaff
    {
        // on click casts a puff of poison gas at the cross hairs in the centre of screen and starts the cooldown timer
        public override void CastSpell()
        {
            if (!CanCast()) return;

            Camera cam = Camera.main;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Vector3 direction = ray.direction;

            Instantiate(projectilePrefab, castPoint.position, Quaternion.LookRotation(direction));

            StartCooldown();
        }
    }
}