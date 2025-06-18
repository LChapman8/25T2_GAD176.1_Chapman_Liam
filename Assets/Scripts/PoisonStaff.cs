using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeaWizard.Weapons;

public class PoisonStaff : BaseStaff
{
    private ParticleSystem poisonVFX;
    private bool isCasting = false;

    // sets the cast point to my VFX for poison cloud in the scene 
    protected override void Start()
    {
        base.Start();
        poisonVFX = castPoint.GetComponentInChildren<ParticleSystem>(true);
        if (poisonVFX != null)
            poisonVFX.Stop();
    }

    // function for starting casting by playing the particle effect
    public override void StartCasting()
    {
        if (CanCast() && poisonVFX != null && !isCasting)
        {
            poisonVFX.Play();
            isCasting = true;
        }
    }
    // function to stop playing the casting particle effect 
    public override void StopCasting()
    {
        if (poisonVFX != null && isCasting)
        {
            poisonVFX.Stop();
            isCasting = false;
        }
    }

    public override void CastSpell()
    {
        // poison staff doesn't use burst casting so not needed.
    }
}
