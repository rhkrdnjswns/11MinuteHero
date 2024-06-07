using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBow : WRangedWeapon //궁수가 사용하는 무기. (09/25)액티브 스킬 제작 완료 후 리팩토링 시에 Bow로 클래스 이름 바꿔야함
{
    [SerializeField] private float activateTime; //화살이 발사된 후 소멸까지의 시간

    protected float arrowAngle;    //화살 갯수에 따른 화살 각도
    public override void InitSkill() //무기 초기화
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
    private void SummonArrow() //화살 쏘는 로직은 코루틴으로 작성하면 프레임 딜레이때문에 동시에 발사되지 않고, 플레이어가 회전하면 각도도 이상하게 나감
    {
#if UNITY_EDITOR
        AttackCount++;
#endif
        float angleY = arrowAngle; //첫 화살의 각도

        for (int i = 0; i < rangedAttackUtility.ShotCount; i++) //화살 개수만큼 반복
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

            angleY += 10f; //다음 화살의 각도 조정
        }
    }
    protected float GetArrowAngle() //발사하는 화살 갯수에 따라 각 화살의 각도를 설정하기 위한 함수 (맨 첫 화살의 각도를 설정해준다)
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
