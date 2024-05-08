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

    [SerializeField] private GameObject swordPrefab; //검 프리팹
    private SwordSkillObject swordSkillObject; //검 오브젝트 클래스 참조

    [SerializeField] private float arrivalSecond; //공전 시간(얼마만에 회전할건지)
    [Range(0,360)]
    [SerializeField] private float degree; //공전 각도(얼마나 회전할건지)
    [SerializeField] private float distance; //플레이어와의 거리

    [SerializeField] private float attackInterval;

    [SerializeField] private float increaseSizeValue;
    [SerializeField] private float radius;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float originRadius;
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

        InitSwordObject();
        swordSkillObject.InitObject(distance, attackInterval, arrivalSecond, degree, rotSpeed);
    }
    public override void ActivateSkill()
    {
        base.ActivateSkill();

        transform.SetParent(null);
        StartCoroutine(Co_FixPos());
        transform.rotation = Quaternion.identity;

        swordSkillObject.SetDamage(currentDamage);
        swordSkillObject.SetAttackRadius(radius);
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
    private void ActivateSkillAction()
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
            ActivateSkillAction();
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
                break;
            default:
                Debug.LogError("UnDefined Type");
                break;
        }
    }
    protected override void ReadActiveCSVData()
    {
        base.ReadActiveCSVData();

        attackInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 8);
        
        radius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 10);
        originRadius = radius;

        increaseSizeValue = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 16);
        rotSpeed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 22);
        arrivalSecond = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 23);

        degree = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 24);
        distance = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 25);
    }
    protected override void ReadEvolutionCSVData()
    {
        base.ReadEvolutionCSVData();

        attackInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 7);

        radius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 9);
        originRadius = radius;

        rotSpeed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 16);
        arrivalSecond = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 17);

        degree = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 18);
        distance = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 19);
    }
}
