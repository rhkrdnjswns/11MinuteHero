using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionSoulHarvest : ActiveAurora
{
    [SerializeField] private ParticleSystem healingParticle;
    [SerializeField] private float recoverValue;

    protected override IEnumerator Co_ActiveSkillAction() //스킬 기능 코루틴
    {
        while (true)
        {
            yield return coolTimeDelay;
            mainParticle.Play();
            int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, collisionArray, ConstDefine.LAYER_MONSTER);
            int killCount = AttackInRangeUtility.GetKillCountAttackLayerInRadius(collisionArray, currentDamage, num);    
#if UNITY_EDITOR
            AttackCount++;
            TotalDamage += currentDamage * num;
#endif
            InGameManager.Instance.Player.RecoverHp(recoverValue * killCount, EApplicableType.Value);
        }
    }
    protected override void SetParticleByRadius()
    {
        base.SetParticleByRadius();

        ParticleSystem.ShapeModule shape = healingParticle.shape;
        shape.radius = sensingRadius;
    }
    protected override void ReadEvolutionCSVData()
    {
        base.ReadEvolutionCSVData();

        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 9);
        originRadius = sensingRadius;

        recoverValue = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 32);
    }
}
