using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRotateSword : ActiveSkill
{
    public enum ESwordType
    {
        BloodSword,
        CurseSword,
        DevilSword
    }

    [SerializeField] private ESwordType eSwordType;

    [SerializeField] private GameObject swordPrefab; //�� ������
    private SwordSkillObject swordSkillObject; //�� ������Ʈ Ŭ���� ����

    [SerializeField] private float arrivalSecond; //���� �ð�(�󸶸��� ȸ���Ұ���)
    [Range(0,360)]
    [SerializeField] private float degree; //���� ����(�󸶳� ȸ���Ұ���)
    [SerializeField] private float distance; //�÷��̾���� �Ÿ�

    [SerializeField] private float attackInterval;

    [SerializeField] private float increaseSizeValue;
    [SerializeField] private float radius;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float originRadius;

    private void Awake()
    {
        InitSwordObject();
    }
    private void InitSwordObject()
    {
        GameObject obj = Instantiate(swordPrefab);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        swordSkillObject = obj.GetComponent<SwordSkillObject>();

        foreach (var item in swordSkillObject.GetAfterImage())
        {
            item.transform.SetParent(transform);
        }
    }

    public override void InitSkill()
    {
        base.InitSkill();

        transform.SetParent(null);
        StartCoroutine(Co_FixPos());
        transform.rotation = Quaternion.identity;

        originRadius = radius;
        swordSkillObject.SetDamage(currentDamage);
        swordSkillObject.SetAttackRadius(radius);
        swordSkillObject.InitObject(distance, attackInterval, arrivalSecond, degree, rotSpeed);
    }
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();
        SetCurrentRange(increaseSizeValue);
    }
    protected override void SetCurrentDamage()
    {
        base.SetCurrentDamage();
        swordSkillObject.SetDamage(currentDamage);
    }
    protected override void SetCurrentRange(float value)
    {
        swordSkillObject.IncreaseSize(value);
        radius += originRadius * value / 100;
        swordSkillObject.SetAttackRadius(radius);
    }
    private void ActivateSkill()
    {
        swordSkillObject.bEndRev = false;
        swordSkillObject.ActivateSkill(transform);
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        WaitUntil endRev = new WaitUntil(() => swordSkillObject.bEndRev);
        while (true)
        {
            yield return coolTimeDelay;
            ActivateSkill();
#if UNITY_EDITOR
            AttackCount++;
#endif
            yield return endRev;
        }
    }
    private IEnumerator Co_FixPos()
    {
        while (true)
        {
            transform.position = InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f;
            yield return null;
        }
    }
    public override void SetEvlotionCondition()
    {
        switch (eSwordType)
        {
            case ESwordType.BloodSword:
                if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.CurseSword).Level == ConstDefine.SKILL_MAX_LEVEL)
                {
                    InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.DevilSword);
                    bCanEvolution = true;
                }
                break;
            case ESwordType.CurseSword:
                if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.BloodSword).Level == ConstDefine.SKILL_MAX_LEVEL)
                {
                    InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.DevilSword);
                    bCanEvolution = true;
                }
                break;
            case ESwordType.DevilSword:
               // Debug.LogError("UnDefined Type");
                break;
            default:
                break;
        }
    }
}