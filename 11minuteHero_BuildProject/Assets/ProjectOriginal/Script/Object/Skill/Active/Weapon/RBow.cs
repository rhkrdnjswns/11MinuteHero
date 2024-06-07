using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBow : WRangedWeapon //�ü��� ����ϴ� ����. (09/25)��Ƽ�� ��ų ���� �Ϸ� �� �����丵 �ÿ� Bow�� Ŭ���� �̸� �ٲ����
{
    [SerializeField] private float activateTime; //ȭ���� �߻�� �� �Ҹ������ �ð�

    protected float arrowAngle;    //ȭ�� ������ ���� ȭ�� ����
    public override void InitSkill() //���� �ʱ�ȭ
    {
        base.InitSkill();

        rangedAttackUtility.SetActivateTime(activateTime);

        UpdateSkillData();
    }

    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        rangedAttackUtility.ShotCount = level;
        rangedAttackUtility.SetCount(level + 2);

        arrowAngle = GetArrowAngle();
    }

    protected override IEnumerator Co_Shot()
    {
        SummonArrow();
        yield return null;
    }
    private void SummonArrow() //ȭ�� ��� ������ �ڷ�ƾ���� �ۼ��ϸ� ������ �����̶����� ���ÿ� �߻���� �ʰ�, �÷��̾ ȸ���ϸ� ������ �̻��ϰ� ����
    {
#if UNITY_EDITOR
        AttackCount++;
#endif
        float angleY = arrowAngle; //ù ȭ���� ����

        for (int i = 0; i < rangedAttackUtility.ShotCount; i++) //ȭ�� ������ŭ �ݺ�
        {
            if (!rangedAttackUtility.IsValid())
            { 
                rangedAttackUtility.CreateNewProjectile();
                rangedAttackUtility.SetActivateTime(activateTime);
                rangedAttackUtility.SetCount(level + 2);
            }
            Projectile p = rangedAttackUtility.SummonProjectile(Quaternion.Euler(0, angleY, 0));

            p.SetShotDirection(p.transform.forward);

            p.ShotProjectile();

            angleY += 10f; //���� ȭ���� ���� ����
        }
    }
    protected float GetArrowAngle() //�߻��ϴ� ȭ�� ������ ���� �� ȭ���� ������ �����ϱ� ���� �Լ� (�� ù ȭ���� ������ �������ش�)
    {
        return rangedAttackUtility.ShotCount > 1 ? -5f + (-(rangedAttackUtility.ShotCount - 2) * 5f) : 0;
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseItemGain) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.LionBow);
            bCanEvolution = true;
        }
    }
}
