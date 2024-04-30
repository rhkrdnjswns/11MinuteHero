using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRedGolemStoneHard : CRedGolemStone
{
    [SerializeField] private Decal earthQuakeDecal;
    [SerializeField] private ParticleSystem earthQuakeParticle;
    [SerializeField] private float radius;

    private Collider[] earthQuakeCollisionArray = new Collider[1];
    private Coroutine earthquakeCoroutine;
    public override void ResetStatus()
    {
        base.ResetStatus();
        earthquakeCoroutine = StartCoroutine(Co_EarthQuake());
        ParticleSystem.MainModule main = earthQuakeParticle.main;

        main.startSizeX = radius / 2;
        main.startSizeZ = radius / 2;
    }
    public override IEnumerator Co_CollectStone(float collectTime)
    {
        StopCoroutine(earthquakeCoroutine);
        earthQuakeDecal.InActiveDecal();
        return base.Co_CollectStone(collectTime);
    }
    private IEnumerator Co_EarthQuake()
    {
        while(gameObject.activeSelf)
        {
            float timer = 0;
            while (timer < 5)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            yield return StartCoroutine(earthQuakeDecal.Co_ActiveDecal(4f, 3f));

            earthQuakeParticle.Play();
            int num = Physics.OverlapSphereNonAlloc(transform.position, 1, earthQuakeCollisionArray, ConstDefine.LAYER_PLAYER);
            AttackInRangeUtility.AttackLayerInRange(earthQuakeCollisionArray, 20, num);
            earthQuakeDecal.InActiveDecal();
        }
    }
}
