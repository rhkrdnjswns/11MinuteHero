using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveRecovery : PassiveSkill //ü�� ��� �нú� ��ų
{
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        StartCoroutine(Co_Recovery());
    }
    protected override void UpdateSkillData()
    {
        SetDescription();
    }
    private IEnumerator Co_Recovery() //ü�� ��� �ڷ�ƾ
    {
        WaitForSeconds delay = new WaitForSeconds(1f);
        while (true)
        {
            yield return delay;
            InGameManager.Instance.Player.RecoverHp(percentage * level, EApplicableType.Percentage);
        }
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Aurora);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }

    protected override void SetDescription()
    {
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("�ʴ� �ִ� ü���� ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%�� ȸ���մϴ�.");

        currentDescriptionStringBuilder.Clear();
        currentDescriptionStringBuilder.Append("�ʴ� �ִ� ü���� ");
        currentDescriptionStringBuilder.Append(percentage * level);
        currentDescriptionStringBuilder.Append("%�� ȸ���մϴ�.");

        base.SetDescription();
    }
}
