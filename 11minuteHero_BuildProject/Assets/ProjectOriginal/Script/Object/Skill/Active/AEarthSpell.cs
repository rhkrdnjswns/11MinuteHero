using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEarthSpell : AActiveSkill //������ ������ ��ų Ŭ����
{
    //������ �������� ��ų �ߵ� �� ���� �ð��� �����ϱ� ������ ���� ������Ʈ�� ����� �� ����.
    //��Ÿ�� �� �ߵ��ϸ� 1.5�� �Ŀ� �������� ����. ��Ÿ�� ������ �ߵ� ��� ���ŵ�

    [SerializeField] private GameObject earthSpellPrefab; //���� ���� ������Ʈ ������
    private Queue<EarthSpellObject> earthSpellQueue = new Queue<EarthSpellObject>();
    [SerializeField] private AttackRadiusUtility attackRadiusUtility;

    private List<EarthSpellObject> allEarthSpellList = new List<EarthSpellObject>();
    private float originRadius;
    [SerializeField] protected float secondDamage;
    private float secondBaseDamage;
    [SerializeField] private float createCount;
    [SerializeField] protected float slowDuration;
    [SerializeField] protected float slowPercentage;


    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < createCount; i++)
        {
            var obj = Instantiate(earthSpellPrefab);
            obj.transform.SetParent(transform);
            var e = obj.GetComponent<EarthSpellObject>().SetAttackRadiusUtility(attackRadiusUtility);
            earthSpellQueue.Enqueue(e);
            allEarthSpellList.Add(e);
        }
        originRadius = attackRadiusUtility.Radius;
        secondBaseDamage = secondDamage;
    }
    public override void IncreaseDamage(float value)
    {
        increaseDamage += value; //���� ������ ����ġ ����
        damage += baseDamage * value / 100; //�տ��� ������ ������Ŵ
        secondDamage += secondBaseDamage * value / 100;
        SetCurrentDamage();
    }
    public override void InitSkill()//������Ʈ ��ġ ���� �� ������Ʈ ���� �ν��Ͻ�ȭ
    {
        base.InitSkill();

        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage + secondDamage * level;
    }
    protected override void SetCurrentRange(float value)
    {
        foreach (var item in allEarthSpellList)
        {
            item.transform.localScale += (Vector3.one - Vector3.up) * (value / 100f);
        }
        attackRadiusUtility.Radius += originRadius * value / 100f;
    }
    protected override IEnumerator Co_ActiveSkillAction() //��ų ��� �ڷ�ƾ 
    {
        while(true)
        {
            yield return new WaitForSeconds(currentCoolTime);
            var e = earthSpellQueue.Dequeue();
            e.transform.SetParent(null);
            e.ActivateSkill(transform, earthSpellQueue, currentDamage, slowDuration, slowPercentage);
        }
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseMoveSpeed) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.EarthImplosion);
            bCanEvolution = true;
        }
    }
}
