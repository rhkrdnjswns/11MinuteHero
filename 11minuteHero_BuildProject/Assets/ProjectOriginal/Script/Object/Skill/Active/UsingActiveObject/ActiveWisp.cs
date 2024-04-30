using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWisp : ActiveSkillUsingActiveObject //������� ��ų Ŭ����
{
    [SerializeField] protected int wispCount; //������� ����
    protected int currentWispCount;
    [SerializeField] private float distanceToPlayer; //������Ұ� �÷��̾� ���� �Ÿ�

    public override void InitSkill() //��� ù ���� �� �ʱ�ȭ. ��������� �ִ밳����ŭ �̸� ��������
    {
        base.InitSkill();

        transform.SetParent(null);
        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();
        currentWispCount = wispCount + level;

        ActivateWisp();
    }
    protected override void SetCurrentDamage()
    {
        base.SetCurrentDamage();
        activeObjectUtility.SetDamage(currentDamage);
    }
    protected void ActivateWisp() //���� ������ �´� ������� n���� ��������
    {
        foreach (var item in activeObjectUtility.AllActiveObjectList)
        {
            if(item.gameObject.activeSelf)
            {
                activeObjectUtility.ReturnActiveObject(item);
            }
        }
        for (int i = 0; i < currentWispCount; i++) //��������� ���� ������ �°� n���� ��ȿ�ϰ� ����
        {
            //�÷��̾ �������� ���� ���͸� ������� ���� / 360���� ������. ù ��° ��������� 0�� ����
            ActiveObject obj = activeObjectUtility.GetObjectMaintainParent();
            Vector3 angleDirection = Quaternion.Euler(0, (360 / currentWispCount) * i, 0) * Vector3.forward;
            obj.transform.localPosition = angleDirection.normalized * distanceToPlayer;
            obj.Activate();
        }
    }
    protected override IEnumerator Co_ActiveSkillAction() //��ų�� Ȱ��ȭ�Ǹ� Ȱ��ȭ�� ������� ������Ʈ���� �θ� ������Ʈ�� �÷��̾��� �����Ǹ� ���󰡰�,
                                                          //ȸ������ ������ �ʵ��� �ϱ� ���� ó��
                                                          //�ش� ó���� �����ָ� �÷��̾��� ȸ������ �ٲ� ������ ������ҵ� ���� ȸ���ϰ� ��
    {
        while (true)
        {
            transform.position = InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f;
            yield return null;
        }
    }
    protected override void InitActiveObject()
    {
        base.InitActiveObject();
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.ReduceDamage) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.FireStorm);
            bCanEvolution = true;
        }
    }
}
