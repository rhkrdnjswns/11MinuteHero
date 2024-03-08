using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWisp : AActiveSkill //������� ��ų Ŭ����
{
    [SerializeField] protected int wispCount; //������� ����
    [SerializeField] private float distanceToPlayer; //������Ұ� �÷��̾� ���� �Ÿ�
    [SerializeField] private GameObject wispPrefab; //������� ������
    [SerializeField] protected List<WispObject> wispList = new List<WispObject>(); //������� Ŭ���� ���� ����Ʈ
    [SerializeField] protected float secondDamage;
    private float secondBaseDamage;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < ConstDefine.SKILL_MAX_LEVEL * 2; i++)
        {
            var obj = Instantiate(wispPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            wispList.Add(obj.GetComponent<WispObject>());
        }
        secondBaseDamage = secondDamage;
    }
    public override void IncreaseDamage(float value)
    {
        increaseDamage += value; //���� ������ ����ġ ����
        damage += baseDamage * value / 100; //�տ��� ������ ������Ŵ
        secondDamage += secondBaseDamage * value / 100;
        SetCurrentDamage();
    }
    public override void InitSkill() //��� ù ���� �� �ʱ�ȭ. ��������� �ִ밳����ŭ �̸� ��������
    {
        base.InitSkill();
        transform.SetParent(null);
        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        wispCount = 1 + level;
        StartCoroutine(Co_SetWispByCount());
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage + secondDamage * level;
        foreach (var item in wispList)
        {
            item.SetDamage(currentDamage);
        }
    }
    protected override void SetCurrentRange(float value)
    {
        foreach (var item in wispList)
        {
            foreach(var child in item.GetComponentsInChildren<Transform>())
            {
                child.localScale += Vector3.one * (value / 100f);
            }
        }
    }
    protected IEnumerator Co_SetWispByCount() //���� ������ �´� ������� n���� ��������
    {
        foreach (var item in wispList) //�ڷ�ƾ �ߺ� ����, ���� �ð� ȿ���� ���� ���� �����ִ� ��������� �� ����
        {
            item.gameObject.SetActive(false);
            yield return null;
        }
        for (int i = 0; i < wispCount; i++) //��������� ���� ������ �°� n���� ��ȿ�ϰ� ����
        {
            //�÷��̾ �������� ���� ���͸� ������� ���� / 360���� ������. ù ��° ��������� 0�� ����
            Vector3 angleDirection = Quaternion.Euler(0, (360 / wispCount) * i, 0) * Vector3.forward;
            //������������ ���������(�������������� �����ϸ� �÷��̾�Լ� ����� ������). normalized�� ���� ���� 1�� ���⺤�͸� ��������
            //�÷��̾�� ����߸� �Ÿ���ŭ ������
            wispList[i].transform.localPosition = angleDirection.normalized * distanceToPlayer;

            wispList[i].gameObject.SetActive(true);
            wispList[i].SetWsip(currentDamage);
            yield return null;
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
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.ReduceDamage) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.FireStorm);
            bCanEvolution = true;
        }
    }
}
