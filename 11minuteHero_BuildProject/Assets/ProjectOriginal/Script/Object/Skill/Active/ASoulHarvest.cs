using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASoulHarvest : AAurora
{
    [SerializeField] private ParticleSystem healingParticle;
    [SerializeField] private float recoverValue;

    private int killCount;
    protected override void SetCurrentDamage()
    {
        currentDamage = damage;
    }
    protected override IEnumerator Co_ActiveSkillAction() //스킬 기능 코루틴
    {
        while (true)
        {
            yield return new WaitForSeconds(currentCoolTime);
            mainParticle.Play();
            killCount = attackRadiusUtility.GetKillCountAttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform.root), currentDamage);
            InGameManager.Instance.Player.RecoverHp(recoverValue * killCount, EApplicableType.None);
        }
    }
    protected override void SetParticleByRadius()
    {
        base.SetParticleByRadius();

        ParticleSystem.ShapeModule shape = healingParticle.shape;
        shape.radius = attackRadiusUtility.Radius;
    }
}
