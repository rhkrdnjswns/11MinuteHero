using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRecovery : SPassive //ü�� ��� �нú� ��ų
{
    [SerializeField] private float recoveryInterval;
    [SerializeField] private float recoveryPercentage;
    public override void InitSkill() //ü�� ����� ��� ��� ���ð� ���ÿ� �۵��ؾ� �ؼ� �ʱ�ȭ �ÿ� ü�� ��� �ڷ�ƾ�� ȣ����.
    {
        base.InitSkill();
        StartCoroutine(Co_Recovery());
        description = $"�ʴ� �ִ� ü���� {recoveryPercentage + 0.25f}%�� ȸ���մϴ�.";
    }
    protected override void UpdateSkillData()
    {
        recoveryPercentage += 0.25f;
        description = $"�ʴ� �ִ� ü���� {(recoveryPercentage + 0.25f > 1 ? 1 : recoveryPercentage + 0.25f)}%�� ȸ���մϴ�.";
    }
    private IEnumerator Co_Recovery() //ü�� ��� �ڷ�ƾ
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
