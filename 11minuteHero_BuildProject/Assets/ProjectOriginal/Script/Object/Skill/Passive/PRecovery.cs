using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRecovery : SPassive //체력 재생 패시브 스킬
{
    [SerializeField] private float recoveryInterval;
    [SerializeField] private float recoveryPercentage;
    public override void InitSkill() //체력 재생의 경우 기믹 선택과 동시에 작동해야 해서 초기화 시에 체력 재생 코루틴을 호출함.
    {
        base.InitSkill();
        StartCoroutine(Co_Recovery());
        description = $"초당 최대 체력의 {recoveryPercentage + 0.25f}%를 회복합니다.";
    }
    protected override void UpdateSkillData()
    {
        recoveryPercentage += 0.25f;
        description = $"초당 최대 체력의 {(recoveryPercentage + 0.25f > 1 ? 1 : recoveryPercentage + 0.25f)}%를 회복합니다.";
    }
    private IEnumerator Co_Recovery() //체력 재생 코루틴
    {
        while (true)
        {
            yield return new WaitForSeconds(recoveryInterval);
            InGameManager.Instance.Player.RecoverHp(recoveryPercentage, EApplicableType.Percentage);
        }
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Aurora);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }
}
