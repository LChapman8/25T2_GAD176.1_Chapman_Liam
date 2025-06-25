using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeaWizard.Weapons;

public class PoisonStaff : BaseStaff
{
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
