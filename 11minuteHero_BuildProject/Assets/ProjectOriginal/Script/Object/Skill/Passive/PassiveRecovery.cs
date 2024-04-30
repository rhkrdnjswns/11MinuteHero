using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveRecovery : PassiveSkill //ü�� ��� �нú� ��ų
{
    public override void InitSkill() //ü�� ����� ��� ��� ���ð� ���ÿ� �۵��ؾ� �ؼ� �ʱ�ȭ �ÿ� ü�� ��� �ڷ�ƾ�� ȣ����.
    {
        base.InitSkill();
        StartCoroutine(Co_Recovery());
    }
    protected override void UpdateSkillData()
    {
        percentage += increaseValueByLevel;
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("�ʴ� �ִ� ü���� ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%�� ȸ���մϴ�.");

        base.UpdateSkillData();
    }
    private IEnumerator Co_Recovery() //ü�� ��� �ڷ�ƾ
    {
        WaitForSeconds delay = new WaitForSeconds(1f);
        while (true)
        {
            yield return delay;
            InGameManager.Instance.Player.RecoverHp(percentage, EApplicableType.Percentage);
        }
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Aurora);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }
}
