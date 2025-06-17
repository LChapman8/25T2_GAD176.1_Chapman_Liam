using SeaWizard.Weapons;
using UnityEngine;

public class FireStaff : BaseStaff
{
    private ParticleSystem fireVFX;
    private bool isCasting = false;

    private void Start()
    {
        fireVFX = castPoint.GetComponentInChildren<ParticleSystem>(true);
        if (fireVFX != null)
            fireVFX.Stop();
    }

    private void Update()
    {
        if (!CanCast()) return;

        if (Input.GetMouseButton(0))
        {
            if (!isCasting)
            {
                isCasting = true;
                if (fireVFX != null)
                    fireVFX.Play();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCasting = false;
            if (fireVFX != null)
                fireVFX.Stop();
        }
    }

    public override void CastSpell()
    {
        // not currently needed
    }
}
