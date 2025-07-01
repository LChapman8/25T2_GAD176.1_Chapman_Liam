using UnityEngine;

namespace SeaWizard.Weapons
{
    public class IceStaff : BaseStaff
    {

        public override void CastSpell()
        {
            if (!CanCast()) return;

            Camera cam = Camera.main;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Vector3 direction = ray.direction;

            GameObject projectile = Instantiate(projectilePrefab, castPoint.position, Quaternion.LookRotation(direction));

            var proj = projectile.GetComponent<StaffProjectile>();
            if (proj != null)
            {
                proj.SetDamage(damage);
                proj.appliesSlow = true;
                proj.slowFactor = 0.5f;
                proj.slowDuration = 2f;
            }

            StartCooldown();
        }
    }
}
