using UnityEngine;
using System.Text;
public abstract class PassiveSkill : Skill //�нú� ��ų Ŭ����.
{
    //��� �нú� ��ų�� ������ �´� ��ġ�� ���� ��ġ�� �ʱ� ��ġ�� percentage%��ŭ ������Ű�ų� ���ҽ�Ų��.
    [SerializeField] protected float percentage; //������
    [SerializeField] protected float increaseValueByLevel;
    public string descriptionCurrentPercentage;

    protected StringBuilder descriptionStringBuilder = new StringBuilder();
    protected StringBuilder currentDescriptionStringBuilder = new StringBuilder();
    protected virtual void SetDescription()
    {
        description = descriptionStringBuilder.ToString();
        descriptionCurrentPercentage = currentDescriptionStringBuilder.ToString();
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
        float value = 0;
        if (level == 0)
        {
            value = increaseValueByLevel;
        }
        else
        {
            value = percentage + increaseValueByLevel * level;
        }
        return value;
    }
    protected override void ReadCSVData()
    {
        base.ReadCSVData();

        percentage = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);
        increaseValueByLevel = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 5);
    }
}
