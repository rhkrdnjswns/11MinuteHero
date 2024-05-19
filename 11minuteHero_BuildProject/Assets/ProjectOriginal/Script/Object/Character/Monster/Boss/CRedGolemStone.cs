using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRedGolemStone : Monster //1스테이지 보스 레드골렘의 돌 오브젝트 (플레이어 공격에 데미지를 입음. 파괴 가능)
{
    public enum EStoneType
    {
        Destructible,
        Indestructible
    }
    public enum EStoneLevel
    {
        Easy = 0,
        Normal,
        Hard
    }

    [SerializeField] private EStoneType eStoneType;
    [SerializeField] private EStoneLevel eStoneLevel;
    private float currentDamage;

    private Transform parent;
    private Transform decalParent;
  //  private DamageUIContainer damageUIContainer = new DamageUIContainer();

    [SerializeField] private Decal chargingDecal;
    [SerializeField] private Decal decal;

    public EStoneLevel StoneLevel { get; }
    private void Start()
    {
        decalParent = transform.GetChild(0).GetComponent<Transform>();
    }
    public IEnumerator Co_SpreadStone(Vector3 direction, float activateTime)
    {
        float timer = 0;
        decalParent.forward = direction;
        decal.transform.position = transform.position + direction * (currentSpeed * activateTime / 2);
        StartCoroutine(decal.Co_ActiveDecal(new Vector3(1,currentSpeed * activateTime,1)));
        yield return new WaitForSeconds(0.5f);
        decal.InActiveDecal(decalParent);

        eCharacterActionable = ECharacterActionable.Actionable;
        currentDamage = 20;
        while (timer < activateTime)
        {
            timer += Time.deltaTime;
            transform.position += direction * currentSpeed * Time.deltaTime;
            yield return null;
        }
        parent.GetComponent<BRedGolem>().ReturnStone(this, (int)eStoneLevel);
    }
    public virtual IEnumerator Co_CollectStone(float collectTime)
    {
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        gameObject.tag = "Untagged";
        decalParent.LookAt(parent);
        chargingDecal.transform.position = transform.position + (parent.position - transform.position).normalized * (Vector3.Distance(transform.position, parent.position) / 2);
        yield return StartCoroutine(chargingDecal.Co_ActiveDecal(new Vector3(1, Vector3.Distance(transform.position, parent.position)), 2f));

        chargingDecal.InActiveDecal(decalParent);

        eCharacterActionable = ECharacterActionable.Actionable;
        currentDamage = 10;

        float timer = 0;
        Vector3 originPos = transform.position;
        while (timer < collectTime)
        {
            transform.position = Vector3.Lerp(originPos, parent.position + Vector3.up * 0.5f, timer / collectTime);
            timer += Time.deltaTime;
            yield return null;
        }
        eCharacterActionable = ECharacterActionable.Unactionable;
    }
    public virtual void ResetStatus() //풀에서 꺼내올 때마다 초기화
    {
        currentHp = maxHp;
        currentSpeed = speed;

        if (eStoneType == EStoneType.Destructible)
        {
            gameObject.layer = LayerMask.NameToLayer("Monster");
            gameObject.tag = "Monster";
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            gameObject.tag = "Untagged";
        }

        eCharacterActionable = ECharacterActionable.Unactionable;
    }
    public CRedGolemStone SetReference(Transform parent)
    {
        this.parent = parent;

        return this;
    }
    public override void Hit(float damage) //몬스터 피격 함수
    {
        if (eStoneType == EStoneType.Indestructible) return;

        base.Hit(damage);
        damageUIContainer.ActiveDamageUI(damage);
        if (currentHp <= 0) parent.GetComponent<BRedGolem>().ReturnStone(this, (int)eStoneLevel);
    }
    public override void KnockBack(float speed, float duration)
    {
        return;
    }
    public override void KnockBack(float speed, float duration, Vector3 direction)
    {
        return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (eCharacterActionable == ECharacterActionable.Unactionable) return;
        if(other.CompareTag(ConstDefine.TAG_PLAYER))
        {
            InGameManager.Instance.Player.Hit(currentDamage);
            parent.GetComponent<BRedGolem>().ReturnStone(this, (int)eStoneLevel);
        }
    }
}
