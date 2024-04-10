using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRedGolemStoneHard : CRedGolemStone
{
    [SerializeField] private Decal earthQuakeDecal;
    [SerializeField] private AttackRadiusUtility attackRadiusUtility;
    [SerializeField] private ParticleSystem earthQuakeParticle;
    public override void ResetStatus()
    {
        base.ResetStatus();
        StartCoroutine(Co_EarthQuake());
        ParticleSystem.MainModule main = earthQuakeParticle.main;

        main.startSizeX = attackRadiusUtility.Radius / 2;
        main.startSizeZ = attackRadiusUtility.Radius / 2;
    }
    private IEnumerator Co_EarthQuake()
    {
        while(gameObject.activeSelf)
        {
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(earthQuakeDecal.Co_ActiveDecal(4f, 3f));

            earthQuakeParticle.Play();
            attackRadiusUtility.AttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform), 20);
            earthQuakeDecal.InActiveDecal();
        }
    }
}
