using UnityEngine;
using SeaWizard.Weapons;

public class IceStaff : BaseStaff
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanCast())
        {
            CastSpell();
        }
    }

    public override void CastSpell()
    {
        if (projectilePrefab == null || castPoint == null) return;

        // ray cast at crosshairs
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
        {
            targetPoint = hitInfo.point;
        }
        else
        {
            // shoot toward crosshairs
            targetPoint = ray.origin + ray.direction * 100f;
        }

        // finds the direction from the view point to the target 
        Vector3 direction = (targetPoint - castPoint.position).normalized;

        // instantiate the projectile prefab and launch it
        GameObject iceProjectile = Instantiate(projectilePrefab, castPoint.position, Quaternion.LookRotation(direction));

        Rigidbody rb = iceProjectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        StartCooldown();
    }
}



