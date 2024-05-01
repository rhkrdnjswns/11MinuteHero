using UnityEngine;
using System.Text;
public abstract class PassiveSkill : Skill //패시브 스킬 클래스.
{
    //모든 패시브 스킬은 종류에 맞는 수치의 현재 수치를 초기 수치의 percentage%만큼 증가시키거나 감소시킨다.
    [SerializeField] protected float percentage; //증감값
    [SerializeField] protected float increaseValueByLevel;

    protected StringBuilder descriptionStringBuilder = new StringBuilder();
    protected virtual void SetDescription()
    {
        description = descriptionStringBuilder.ToString();
    }
    public override void InitSkill()
    {
        base.InitSkill();

        SetDescription();
    }
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        UpdateSkillData();
    }
    protected float GetPercentageForDescription()
    {
        return percentage * (level + 1) > percentage * ConstDefine.SKILL_MAX_LEVEL ? percentage * ConstDefine.SKILL_MAX_LEVEL : percentage * (level + 1);
    }
    protected override void ReadCSVData()
    {
        base.ReadCSVData();

        percentage = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);
        increaseValueByLevel = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 5);
    }
}
