using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWisp : ActiveSkillUsingActiveObject //도깨비불 스킬 클래스
{
    [SerializeField] protected int wispCount; //도깨비불 개수
    protected int currentWispCount;
    [SerializeField] private float distanceToPlayer; //도깨비불과 플레이어 사이 거리

    public override void InitSkill() //기믹 첫 선택 시 초기화. 도깨비불을 최대개수만큼 미리 만들어놓음
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
    protected void ActivateWisp() //현재 레벨에 맞는 도깨비불 n개를 설정해줌
    {
        foreach (var item in activeObjectUtility.AllActiveObjectList)
        {
            if(item.gameObject.activeSelf)
            {
                activeObjectUtility.ReturnActiveObject(item);
            }
        }
        for (int i = 0; i < currentWispCount; i++) //도깨비불을 현재 레벨에 맞게 n개를 유효하게 해줌
        {
            //플레이어를 기준으로 방향 벡터를 도깨비불 갯수 / 360으로 구해줌. 첫 번째 도깨비불은 0이 나옴
            ActiveObject obj = activeObjectUtility.GetObjectMaintainParent();
            Vector3 angleDirection = Quaternion.Euler(0, (360 / currentWispCount) * i, 0) * Vector3.forward;
            obj.transform.localPosition = angleDirection.normalized * distanceToPlayer;
            obj.Activate();
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
