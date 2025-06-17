using UnityEngine;

namespace SeaWizard.Weapons
{
    public class IceStaff : BaseStaff
    {
        // logic for casting the icebolt from the ice staff at the crosshairs using raycasting from the camera
        // then instantiates my icebolt prefab and then gives its rigid body velocity to move it
        public override void CastSpell()
        {
            if (!CanCast()) return;

            Camera cam = Camera.main;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Vector3 direction = ray.direction;

            GameObject iceProjectile = Instantiate(projectilePrefab, castPoint.position, Quaternion.LookRotation(direction));

            Rigidbody rb = iceProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }

            StartCooldown();
        }
    }
}
