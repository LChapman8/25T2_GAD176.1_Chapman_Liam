using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeaWizard.Weapons;

public class IceStaff : BaseStaff
{
    public override void CastSpell()
    {
        if (CanCast())
        {
            GameObject iceProjectile = Instantiate(projectilePrefab, castPoint.position, castPoint.rotation);

            Rigidbody rb = iceProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = castPoint.forward * projectileSpeed;
            }
            StartCooldown();
        }
    }
}


