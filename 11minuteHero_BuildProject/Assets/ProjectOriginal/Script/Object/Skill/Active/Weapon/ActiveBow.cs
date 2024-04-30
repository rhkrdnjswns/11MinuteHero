using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBow : ActiveSkillUsingProjectile //궁수가 사용하는 무기. (09/25)액티브 스킬 제작 완료 후 리팩토링 시에 Bow로 클래스 이름 바꿔야함
{
    [SerializeField] private int penetrationCount;
    [SerializeField] private int currentPenetrationCount;
    [SerializeField] private float activateTime;

    protected float arrowAngle;    //화살 갯수에 따른 화살 각도
    public override void ActivateSkill()
    {
        base.ActivateSkill();

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
    private void SummonArrow() //화살 쏘는 로직은 코루틴으로 작성하면 프레임 딜레이때문에 동시에 발사되지 않고, 플레이어가 회전하면 각도도 이상하게 나감
    {
#if UNITY_EDITOR
        AttackCount++;
#endif
        float angleY = arrowAngle; //첫 화살의 각도

        for (int i = 0; i < currentShotCount; i++) //화살 개수만큼 반복
        {
            if (!projectileUtility.IsValid())
            {
                InitProjectile();
            }
            Projectile p =  projectileUtility.GetProjectile(Quaternion.Euler(0,angleY,0));

            p.SetShotDirection(p.transform.forward);

            p.ShotProjectile();

            angleY += 10f; //다음 화살의 각도 조정
        }
    }
    protected float GetArrowAngle() //발사하는 화살 갯수에 따라 각 화살의 각도를 설정하기 위한 함수 (맨 첫 화살의 각도를 설정해준다)
    {
        return currentShotCount > 1 ? -5f + (-(currentShotCount - 2) * 5f) : 0;
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
    protected override void ReadCSVData()
    {
        base.ReadCSVData();

        if (eSkillType == ESkillType.Evolution) return;

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 14);
        currentShotCount = shotCount;

        activateTime = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 17);
        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 18);

        penetrationCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 21);
        currentPenetrationCount = penetrationCount;
    }
}
