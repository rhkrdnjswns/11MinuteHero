using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRenotoros : AMagicShield
{
    [SerializeField] private int shotCount;
    private byte returnCount;
    public byte ReturnCount { get => returnCount; set => returnCount = value; }
    public override void InitSkill()
    {
        base.InitSkill();
        rangedAttackUtility.ShotCount = shotCount;
        foreach (var item in rangedAttackUtility.AllProjectileList)
        {
            item.GetComponent<PRenotoros>().SetReference(this);
        }
    }
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage;
        rangedAttackUtility.SetDamage(CurrentDamage);
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        WaitUntil bReturn = new WaitUntil(() => returnCount == 2);
        while (true)
        {
            yield return coolTimeDelay;
            ShotProjectile();
            yield return bReturn;
        }
    }
    private void ShotProjectile()
    {
        Projectile p;
        returnCount = 0;
#if UNITY_EDITOR
        AttackCount++;
#endif
        for (int i = 0; i < rangedAttackUtility.ShotCount; i++)
        {
            if (!rangedAttackUtility.IsValid()) rangedAttackUtility.CreateNewProjectile();
            p = rangedAttackUtility.SummonProjectile();
            if (i % 2 == 0) //왼쪽 오른쪽 번갈아가면서 소환
            {
                p.SetShotDirection((Vector3.forward + Vector3.right).normalized);
            }
            else
            {
                p.SetShotDirection((Vector3.left + Vector3.back).normalized);
            }

            p.ShotProjectile();
        }
    }
}
