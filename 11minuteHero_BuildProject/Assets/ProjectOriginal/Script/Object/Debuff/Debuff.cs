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
            if (item.EDebuffType == debuff.EDebuffType) //�̹� �ش� ������� �����ϴ� ��� ���� �ð� �ʱ�ȭ
            {
                item.CurrentTime = 0;
                return;
            }
        }
        debuffList.Add(debuff); //�������� �ʴ� ��� ����Ʈ�� �߰�
        debuff.DebuffListReference = debuffList; //���۷��� �Ѱ��� (���ӽð� ���� �� ���� �ɷ�ġ �ʱ�ȭ�� ����)
        owner.StartCoroutine(debuff.Co_ApplyEffect()); //����� ȿ�� �ڷ�ƾ ����
    }
}
public abstract class Debuff
{
    protected EDebuffType eDebuffType; //����� Ÿ��
    protected float applyTime; //����� ���� �ð�
    protected float currentTime; //����� ���� �ð�

    protected List<Debuff> debuffListReference; //����� ����Ʈ ���۷���
    protected Monster characterReference; //������� ������ Ÿ�� ����
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
    public abstract IEnumerator Co_ApplyEffect(); //����� ȿ�� �ڷ�ƾ
}
public class DSlowDown : Debuff //��ȭ
{
    private float value; //���� ��ġ
    public DSlowDown(EDebuffType type, float time, Monster monster, float value = 0) : base(type, time, monster)
    {
        this.value = value;
    }

    public override IEnumerator Co_ApplyEffect() //����� ȿ�� ����
    {
        characterReference.CurrentSpeed -= value;
        while(currentTime < applyTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        characterReference.CurrentSpeed += value;
        debuffListReference.Remove(this); //����� ����Ʈ���� ����
    }
}
public class DSturn : Debuff //����
{
    public DSturn(EDebuffType type, float time, Monster monster) : base(type, time, monster)
    {

    }
    public override IEnumerator Co_ApplyEffect() //����� ȿ�� ����
    {
        characterReference.ECharacterActionable = ECharacterActionable.Unactionable;
        while (currentTime < applyTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        characterReference.ECharacterActionable = ECharacterActionable.Actionable;
        debuffListReference.Remove(this); //����� ����Ʈ���� ����
    }
}