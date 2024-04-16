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
        CurrentDamage = damage;
    }
    protected override IEnumerator Co_ActiveSkillAction() //스킬 기능 코루틴
    {
        while (true)
        {
            yield return coolTimeDelay;
            mainParticle.Play();
            killCount = attackRadiusUtility.GetKillCountAttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform.root), CurrentDamage);
#if UNITY_EDITOR
            AttackCount++;
            int count = attackRadiusUtility.GetLayerInRadius(transform.root).Length;
            TotalDamage += CurrentDamage * count;
#endif
            InGameManager.Instance.Player.RecoverHp(recoverValue * killCount, EApplicableType.Value);
        }
    }
    protected override void SetParticleByRadius()
    {
        base.SetParticleByRadius();

        ParticleSystem.ShapeModule shape = healingParticle.shape;
        shape.radius = attackRadiusUtility.Radius;
    }
}
