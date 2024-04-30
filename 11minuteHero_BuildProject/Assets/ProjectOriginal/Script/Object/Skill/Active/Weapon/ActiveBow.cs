using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBow : ActiveSkillUsingProjectile //�ü��� ����ϴ� ����. (09/25)��Ƽ�� ��ų ���� �Ϸ� �� �����丵 �ÿ� Bow�� Ŭ���� �̸� �ٲ����
{
    [SerializeField] private int penetrationCount;
    [SerializeField] private int currentPenetrationCount;
    [SerializeField] private float activateTime;

    protected float arrowAngle;    //ȭ�� ������ ���� ȭ�� ����
    public override void InitSkill() //���� �ʱ�ȭ
    {
        base.InitSkill();

        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();

        currentShotCount = shotCount * level;
        currentPenetrationCount = penetrationCount + level;
        projectileUtility.SetPenetrationCount(currentPenetrationCount);

        arrowAngle = GetArrowAngle();
    }
    private void SummonArrow() //ȭ�� ��� ������ �ڷ�ƾ���� �ۼ��ϸ� ������ �����̶����� ���ÿ� �߻���� �ʰ�, �÷��̾ ȸ���ϸ� ������ �̻��ϰ� ����
    {
#if UNITY_EDITOR
        AttackCount++;
#endif
        float angleY = arrowAngle; //ù ȭ���� ����

        for (int i = 0; i < shotCount; i++) //ȭ�� ������ŭ �ݺ�
        {
            if (!projectileUtility.IsValid())
            {
                InitProjectile();
            }
            Projectile p =  projectileUtility.GetProjectile(Quaternion.Euler(0,angleY,0));

            p.SetShotDirection(p.transform.forward);

            p.ShotProjectile();

            angleY += 10f; //���� ȭ���� ���� ����
        }
    }
    protected float GetArrowAngle() //�߻��ϴ� ȭ�� ������ ���� �� ȭ���� ������ �����ϱ� ���� �Լ� (�� ù ȭ���� ������ �������ش�)
    {
        return shotCount > 1 ? -5f + (-(shotCount - 2) * 5f) : 0;
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseItemGain) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.LionBow);
            bCanEvolution = true;
        }
    }

    protected override IEnumerator Co_ActiveSkillAction()
    {
        while(true)
        {
            yield return coolTimeDelay;

            if (InGameManager.Instance.Player.IsDodge) continue;

            SummonArrow();
        }
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();
        projectileUtility.SetActivateTime(activateTime);
    }
}
