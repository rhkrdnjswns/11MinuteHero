using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDebuffType
{
    None = 0,
    Slow,
    Sturn
}
public class DebuffList
{
    private List<Debuff> debuffList;
    private MonoBehaviour owner;
    public DebuffList(MonoBehaviour owner)
    {
        debuffList = new List<Debuff>();
        this.owner = owner;
    }
    public void Add(Debuff debuff)
    {
        foreach(var item in debuffList)
        {
            if (item.EDebuffType == debuff.EDebuffType) //이미 해당 디버프가 존재하는 경우 적용 시간 초기화
            {
                item.CurrentTime = 0;
                return;
            }
        }
        debuffList.Add(debuff); //존재하지 않는 경우 리스트에 추가
        debuff.DebuffListReference = debuffList; //레퍼런스 넘겨줌 (지속시간 끝난 후 관련 능력치 초기화를 위해)
        owner.StartCoroutine(debuff.Co_ApplyEffect()); //디버프 효과 코루틴 시작
    }
}
public abstract class Debuff
{
    protected EDebuffType eDebuffType; //디버프 타입
    protected float applyTime; //디버프 지속 시간
    protected float currentTime; //디버프 현재 시간

    protected List<Debuff> debuffListReference; //디버프 리스트 레퍼런스
    protected Monster characterReference; //디버프를 적용할 타겟 참조
    public Debuff(EDebuffType type, float time, Monster monster, float value = 0)
    {
        eDebuffType = type;
        applyTime = time;
        characterReference = monster;
    }
    public float ApplyTime { get => applyTime; }
    public EDebuffType EDebuffType { get => eDebuffType; }
    public float CurrentTime { set => currentTime = value; }
    public List<Debuff> DebuffListReference { set => debuffListReference = value; }
    public abstract IEnumerator Co_ApplyEffect(); //디버프 효과 코루틴
}
public class DSlowDown : Debuff //둔화
{
    private float value; //적용 수치
    public DSlowDown(EDebuffType type, float time, Monster monster, float value = 0) : base(type, time, monster)
    {
        this.value = value;
    }

    public override IEnumerator Co_ApplyEffect() //디버프 효과 적용
    {
        characterReference.CurrentSpeed -= value;
        while(currentTime < applyTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        characterReference.CurrentSpeed += value;
        debuffListReference.Remove(this); //디버프 리스트에서 삭제
    }
}
public class DSturn : Debuff //스턴
{
    public DSturn(EDebuffType type, float time, Monster monster) : base(type, time, monster)
    {

    }
    public override IEnumerator Co_ApplyEffect() //디버프 효과 적용
    {
        characterReference.ECharacterActionable = ECharacterActionable.Unactionable;
        while (currentTime < applyTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        characterReference.ECharacterActionable = ECharacterActionable.Actionable;
        debuffListReference.Remove(this); //디버프 리스트에서 삭제
    }
}