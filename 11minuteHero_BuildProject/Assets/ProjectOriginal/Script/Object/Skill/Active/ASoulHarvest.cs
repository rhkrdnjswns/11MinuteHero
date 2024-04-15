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
    protected override IEnumerator Co_ActiveSkillAction() //��ų ��� �ڷ�ƾ
    {
        while (true)
        {
            yield return coolTimeDelay;
            mainParticle.Play();
            killCount = attackRadiusUtility.GetKillCountAttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform.root), currentDamage);
#if UNITY_EDITOR
            AttackCount++;
            int count = attackRadiusUtility.GetLayerInRadius(transform.root).Length;
            TotalDamage += currentDamage * count;
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
