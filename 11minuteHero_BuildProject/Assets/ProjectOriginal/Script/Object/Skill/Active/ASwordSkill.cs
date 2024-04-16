using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASwordSkill : AActiveSkill
{
    public enum ESwordType
    {
        BloodSword,
        CurseSword,
        DevilSword
    }

    [SerializeField] private ESwordType eSwordType;
    [SerializeField] private AttackRadiusUtility attackRadiusUtility; //�ݰ� ���� ��� Ŭ����
    [SerializeField] private GameObject swordPrefab; //�� ������
    private SwordSkillObject swordSkillObject; //�� ������Ʈ Ŭ���� ����

    [SerializeField] private float arrivalSecond; //���� �ð�(�󸶸��� ȸ���Ұ���)
    [Range(0,360)]
    [SerializeField] private float degree; //���� ����(�󸶳� ȸ���Ұ���)
    [SerializeField] private float distance; //�÷��̾���� �Ÿ�

    [SerializeField] private float attackInterval;
    private Vector3 IncreaseSize = Vector3.one * 0.125f;
    private float originRadius;
    [SerializeField] protected float secondDamage;
    private float secondBaseDamage;

    protected override void Awake()
    {
        base.Awake();
        GameObject obj = Instantiate(swordPrefab);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        swordSkillObject = obj.GetComponent<SwordSkillObject>();
        originRadius = attackRadiusUtility.Radius;
        secondBaseDamage = secondDamage;
        foreach (var item in swordSkillObject.GetAfterImage())
        {
            item.transform.SetParent(transform);
        }
    }
    public override void IncreaseDamage(float value)
    {
        increaseDamage += value; //���� ������ ����ġ ����
        damage += baseDamage * value / 100; //�տ��� ������ ������Ŵ
        secondDamage += secondBaseDamage * value / 100;
        SetCurrentDamage();
    }
    public override void InitSkill()
    {
        base.InitSkill();

        transform.SetParent(null);
        StartCoroutine(Co_FixPos());
        transform.rotation = Quaternion.identity;

        swordSkillObject.InitObject(attackRadiusUtility, distance);

        SetCurrentDamage();
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        swordSkillObject.transform.localScale += IncreaseSize;
        attackRadiusUtility.Radius += 0.2f;
        swordSkillObject.SetAfterImageSize();
    }
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage + secondDamage * level;
    }
    protected override void SetCurrentRange(float value)
    {
        swordSkillObject.transform.localScale += Vector3.one * (value / 100f);
        attackRadiusUtility.Radius += originRadius * value / 100f;
        swordSkillObject.SetAfterImageSize();
    }
    private void ActivateSkill()
    {
        swordSkillObject.bEndRev = false;
        swordSkillObject.ActivateSkill(transform, arrivalSecond, degree, CurrentDamage, attackInterval);
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
