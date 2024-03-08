using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWisp : AActiveSkill //도깨비불 스킬 클래스
{
    [SerializeField] protected int wispCount; //도깨비불 개수
    [SerializeField] private float distanceToPlayer; //도깨비불과 플레이어 사이 거리
    [SerializeField] private GameObject wispPrefab; //도깨비불 프리팹
    [SerializeField] protected List<WispObject> wispList = new List<WispObject>(); //도깨비불 클래스 참조 리스트
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
        increaseDamage += value; //현재 데미지 증가치 갱신
        damage += baseDamage * value / 100; //합연산 데미지 증가시킴
        secondDamage += secondBaseDamage * value / 100;
        SetCurrentDamage();
    }
    public override void InitSkill() //기믹 첫 선택 시 초기화. 도깨비불을 최대개수만큼 미리 만들어놓음
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
    protected IEnumerator Co_SetWispByCount() //현재 레벨에 맞는 도깨비불 n개를 설정해줌
    {
        foreach (var item in wispList) //코루틴 중복 방지, 생성 시각 효과를 위해 먼저 켜져있던 도깨비불을 다 꺼줌
        {
            item.gameObject.SetActive(false);
            yield return null;
        }
        for (int i = 0; i < wispCount; i++) //도깨비불을 현재 레벨에 맞게 n개를 유효하게 해줌
        {
            //플레이어를 기준으로 방향 벡터를 도깨비불 갯수 / 360으로 구해줌. 첫 번째 도깨비불은 0이 나옴
            Vector3 angleDirection = Quaternion.Euler(0, (360 / wispCount) * i, 0) * Vector3.forward;
            //로컬포지션을 변경시켜줌(월드포지션으로 변경하면 플레이어에게서 벗어나기 때문에). normalized를 통해 값이 1인 방향벡터를 가져오고
            //플레이어와 떨어뜨릴 거리만큼 곱해줌
            wispList[i].transform.localPosition = angleDirection.normalized * distanceToPlayer;

            wispList[i].gameObject.SetActive(true);
            wispList[i].SetWsip(currentDamage);
            yield return null;
        }
    }
    protected override IEnumerator Co_ActiveSkillAction() //스킬이 활성화되면 활성화될 도깨비불 오브젝트들의 부모 오브젝트가 플레이어의 포지션만 따라가고,
                                                          //회전값은 변하지 않도록 하기 위한 처리
                                                          //해당 처리를 안해주면 플레이어의 회전각이 바뀔 때마다 도깨비불도 따라서 회전하게 됨
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
