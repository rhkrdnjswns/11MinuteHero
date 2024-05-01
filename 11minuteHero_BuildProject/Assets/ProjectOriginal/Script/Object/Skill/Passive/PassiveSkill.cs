using UnityEngine;
using System.Text;
public abstract class PassiveSkill : Skill //�нú� ��ų Ŭ����.
{
    //��� �нú� ��ų�� ������ �´� ��ġ�� ���� ��ġ�� �ʱ� ��ġ�� percentage%��ŭ ������Ű�ų� ���ҽ�Ų��.
    [SerializeField] protected float percentage; //������
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
