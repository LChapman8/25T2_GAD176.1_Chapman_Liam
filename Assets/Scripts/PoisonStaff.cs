using UnityEngine;

namespace SeaWizard.Weapons
{
    public class PoisonStaff : BaseStaff
    {

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