using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BRedGolem : Boss
{
    protected enum EDecalNumber
    {
        Jump = 0,
        KnockbackPunch,
        SummonStoneRandomPos,
        SummonStone,
        SummonStoneX = 3,
        SummonStoneZ
    }
    private bool bAction;
    protected bool bReturnStone;

    [SerializeField] protected RangedAttackUtility rangedAttackUtility;

    [SerializeField] private GameObject stonePrefab;

    protected List<CRedGolemStone> stoneList = new List<CRedGolemStone>();
    protected List<CRedGolemStone> spareStoneList = new List<CRedGolemStone>();
    protected Queue<CRedGolemStone> stoneQueue = new Queue<CRedGolemStone>();

    protected CRedGolemStone stoneRef;

    protected Vector3 appearPos;

    protected Vector3 stoneSummonPos = Vector3.zero;
    protected float summonedStonePosY;

    protected Collider[] jumpAttackCollisionArray = new Collider[1];
    protected Collider[] summonStoneCollisionArray = new Collider[1];
    protected Collider[] summonStoneRandCollisionArray = new Collider[1];
    protected Collider[] punchCollisionArray = new Collider[1];
    protected virtual void SetSummonedStonePosY()
    {
        summonedStonePosY = 0.5f;
    }
    public override void InitBoss()
    {
        appearPos = transform.position;
        SetSummonedStonePosY();
        base.InitBoss();
        StartCoroutine(Co_SummonStoneRandomPos());
    }
    protected override void PlayHpEvent(int index)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(Co_HpEvent_Release());
                break;
            default:
                Debug.LogError($"{this}\nPlayerHpEvent : Index Out Range");
                break;
        }
    }
    protected IEnumerator Co_HpEvent_Release()
    {
        yield return new WaitUntil(() => !bAction);
        Debug.Log("Hp 이벤트 실행");
        yield return new WaitForSeconds(1f);

        Debug.Log("중앙으로 점프");
        transform.forward = (appearPos - transform.position).normalized;

        animator.SetTrigger("JumpStart");

        while (modelObject.activeSelf)
        {
            transform.position += Vector3.up * 10 * Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(cameraUtility.Co_FocusCam(1f, InGameManager.Instance.Player.transform.position, appearPos));
        transform.position = appearPos;
        animator.SetTrigger("JumpEndNonAttack");

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(cameraUtility.Co_FocusCam(0.2f, appearPos, InGameManager.Instance.Player.transform.position));
        cameraUtility.UnFocus();

        transform.LookAt(InGameManager.Instance.Player.transform);
        animator.SetTrigger("SummonStoneRelease");

        for (int i = 0; i < 24; i++)
        {
            Vector3 rotDir = Quaternion.Euler(0, i * 15, 0) * Vector3.forward;
            SummonStone(transform.position + rotDir.normalized * 40);
        }

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(Co_Behavior_CollectStone());

        yield return new WaitForSeconds(2f);

        bHpEvent = false;
    }
    protected override void InitBehaviorTree()
    {
        behaviorTree = new BehaviorTree
            (
               new Condition
               (
                   () => !bAction,
                   new Selector
                   (
                       new Condition(IsInShortRange, new Action(ShortRangeBehavior)),
                       new Condition(IsInMiddleRange, new Action(MiddleRangeBehavior)),
                       new Action(LongRangeBehavior)
                   )
               )
           );
    }
    protected override void Awake()
    {
        base.Awake();
        InitStoneQueue();
        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile();
        rangedAttackUtility.SetDamage(10);
    }
    protected virtual void InitStoneQueue()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = Instantiate(stonePrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;

            CRedGolemStone stone = obj.GetComponent<CRedGolemStone>().SetReference(transform);
            stone.InitDamageUIContainer();
            stoneQueue.Enqueue(stone);
        }
    }
    private bool IsInShortRange()
    {
        float dist = Vector3.Distance(transform.position, InGameManager.Instance.Player.transform.position);
        return dist <= 4;
    }
    private bool IsInMiddleRange()
    {
        float dist = Vector3.Distance(transform.position, InGameManager.Instance.Player.transform.position);
        return dist > 4 && dist <= 12;
    }
    private void ShortRangeBehavior()
    {
        bAction = true;
        StartCoroutine(Co_ShortRangeBehavior());
    }
    private void MiddleRangeBehavior()
    {
        bAction = true;
        StartCoroutine(Co_MiddleRangeBehavior());
    }
    private void LongRangeBehavior()
    {
        bAction = true;
        StartCoroutine(Co_LongRangeBehavior());
    }
    private IEnumerator Co_SummonStoneRandomPos()
    {
        while(true)
        {
            Vector3 randomPos = new Vector3(appearPos.x + Random.Range(-(bossAreaWidth / 2), bossAreaWidth / 2), 0, appearPos.z + Random.Range(-(bossAreaHeight / 2), bossAreaHeight / 2));
            yield return new WaitForSeconds(5f);
            Debug.Log(randomPos);

            decalList[(int)EDecalNumber.SummonStoneRandomPos].transform.SetParent(null);
            decalList[(int)EDecalNumber.SummonStoneRandomPos].transform.position = randomPos;
            decalList[(int)EDecalNumber.SummonStoneRandomPos].gameObject.SetActive(true);
            StartCoroutine(decalList[(int)EDecalNumber.SummonStoneRandomPos].Co_ActiveDecal(2));

            yield return new WaitForSeconds(2f);

            int num = Physics.OverlapSphereNonAlloc(decalList[(int)EDecalNumber.SummonStoneRandomPos].transform.position, 1, summonStoneRandCollisionArray, ConstDefine.LAYER_PLAYER);
            AttackInRangeUtility.AttackLayerInRange(summonStoneRandCollisionArray, 10, num);
            SummonStone(decalList[(int)EDecalNumber.SummonStoneRandomPos].transform.position);
            decalList[(int)EDecalNumber.SummonStoneRandomPos].InActiveDecal(transform);
        }
    }
    private int GetBehaviorByWeight(params int[] weights)
    {
        int rand = Random.Range(0, weights.Sum());

        int index = 0;

        int weight = weights[index];

        while (true)
        {
            if (rand < weight)
            {
                return index;
            }
            else
            {
                weight += weights[++index];
            }
        }
    }
    private IEnumerator Co_ShortRangeBehavior()
    {
        int rand = GetBehaviorByWeight(60, 30, stoneList.Count * 10);
        switch (rand)
        {
            case 0:
                yield return StartCoroutine(Co_Behavior_KnockBackPunch());
                break;
            case 1:
                yield return StartCoroutine(Co_Behavior_SummonStone());
                break;
            case 2:
                yield return StartCoroutine(Co_Behavior_CollectStone());
                break;
            default:
                Debug.LogError("Index Out Range\n근접 패턴 가중치값이 범위를 벗어났습니다. 가중치 값을 확인하세요.");
                break;
        }
        yield return new WaitForSeconds(2f);

        bAction = false;
    }
    private IEnumerator Co_MiddleRangeBehavior()
    {
        int rand = GetBehaviorByWeight(50, 45, stoneList.Count * 10, 5);
        switch (rand)
        {
            case 0:
                yield return StartCoroutine(Co_Behavior_ThrowStone());
                break;
            case 1:
                yield return StartCoroutine(Co_Behavior_SummonStone());
                break;
            case 2:
                yield return StartCoroutine(Co_Behavior_CollectStone());
                break;
            case 3:
                yield return StartCoroutine(Co_Behavior_Jump());
                break;
            default:
                Debug.LogError("Index Out Range\n중거리 패턴 가중치값이 범위를 벗어났습니다. 가중치 값을 확인하세요.");
                break;
        }
        yield return new WaitForSeconds(2f);

        bAction = false;
    }
    private IEnumerator Co_LongRangeBehavior()
    {
        int rand = GetBehaviorByWeight(60, stoneList.Count * 10, 20, 20);
        switch (rand)
        {
            case 0:
                yield return StartCoroutine(Co_Behavior_Move());
                break;
            case 1:
                yield return StartCoroutine(Co_Behavior_CollectStone());
                break;
            case 2:
                yield return StartCoroutine(Co_Behavior_SummonStone());
                break;
            case 3:
                yield return StartCoroutine(Co_Behavior_Jump());
                break;
            default:
                Debug.LogError("Index Out Range\n원거리 패턴 가중치값이 범위를 벗어났습니다. 가중치 값을 확인하세요.");
                break;
        }
        yield return new WaitForSeconds(2f);

        bAction = false;
    }
    protected virtual IEnumerator Co_Behavior_SummonStone()
    {
        Debug.Log("돌 소환");
        transform.LookAt(InGameManager.Instance.Player.transform);
        animator.SetTrigger("SummonStone");

        decalList[(int)EDecalNumber.SummonStone].transform.SetParent(null);
        decalList[(int)EDecalNumber.SummonStone].transform.position = InGameManager.Instance.Player.transform.position;
        StartCoroutine(decalList[(int)EDecalNumber.SummonStone].Co_ActiveDecal(2));

        yield return null;
    }
    private IEnumerator Co_Behavior_SpreadStone()
    {
        Debug.Log("돌 뿌리기");
        foreach (var item in stoneList)
        {
            StartCoroutine(item.Co_SpreadStone(Quaternion.Euler(0,Random.Range(0,361),0) * Vector3.forward, Random.Range(3f,4.6f)));
        }
        stoneList.Clear();
        yield return null;
    }
    private IEnumerator Co_Behavior_CollectStone()
    {
        Debug.Log("돌 모으기");
        if (stoneList.Count == 0) yield break;
        bReturnStone = true;
        animator.SetTrigger("ReturnStone");
        animator.SetBool("IsEndReturn", false);
        foreach (var item in stoneList)
        {
            StartCoroutine(item.Co_CollectStone(2));
            //yield return null;
        }
        for (int i = 0; i < stoneList.Count; i++)
        {
            StartCoroutine(stoneList[i].Co_CollectStone(2));
            if(i < stoneList.Count - 1)
            {
                StartCoroutine(stoneList[i].Co_CollectStone(2));
            }
            else
            {
                yield return StartCoroutine(stoneList[i].Co_CollectStone(2));
            }
        }
        IncreaseHp(5 * stoneList.Count);

        if(stoneList.Count >= 5)
        {
            animator.SetBool("bSpreadStone", true);
            animator.SetBool("IsEndReturn", true);
            yield return StartCoroutine(Co_Behavior_SpreadStone());
        }
        else
        {
            animator.SetBool("bSpreadStone", false);
            animator.SetBool("IsEndReturn", true);
            while (stoneList.Count != 0)
            {
                ReturnStone(stoneList[0], (int)stoneList[0].StoneLevel);
                yield return null;
            }
        }

        bReturnStone = false;
        foreach(var item in spareStoneList)
        {
            stoneList.Add(item);
        }
        spareStoneList.Clear();
    }
    private IEnumerator Co_Behavior_KnockBackPunch()
    {
        Debug.Log("펀치");
        transform.LookAt(InGameManager.Instance.Player.transform);

        decalList[(int)EDecalNumber.KnockbackPunch].transform.SetParent(null);
        decalList[(int)EDecalNumber.KnockbackPunch].transform.position = transform.position + transform.forward * 2;
        yield return StartCoroutine(decalList[(int)EDecalNumber.KnockbackPunch].Co_ActiveDecal(new Vector3(4,6,1), 1f));
        decalList[(int)EDecalNumber.KnockbackPunch].InActiveDecal(transform);

        animator.SetTrigger("Punch");
        int num = Physics.OverlapBoxNonAlloc(transform.position + transform.forward * 2, new Vector3(2, 1, 3), punchCollisionArray, transform.rotation, ConstDefine.LAYER_PLAYER);
        if(num > 0)
        {
            AttackInRangeUtility.AttackLayerInRange(punchCollisionArray, 10, num);
            InGameManager.Instance.Player.KnockBack(5f, 1f, transform.forward); //플레이어 넉백
        }
    }
    private IEnumerator Co_Behavior_ThrowStone()
    {
        Debug.Log("돌 던지기");
        transform.LookAt(InGameManager.Instance.Player.transform);
        animator.SetTrigger("ThrowStone");

        yield return null;
    }
    private IEnumerator Co_Behavior_Jump()
    {
        Debug.Log("점프");
        gameObject.layer = LayerMask.NameToLayer("Default");
        transform.LookAt(InGameManager.Instance.Player.transform);
        animator.SetTrigger("JumpStart");

        while (modelObject.activeSelf)
        {
            transform.position += Vector3.up * 10 * Time.deltaTime;
            yield return null;
        }

        decalList[(int)EDecalNumber.Jump].transform.SetParent(null);
        decalList[(int)EDecalNumber.Jump].transform.position = InGameManager.Instance.Player.transform.position;
        yield return StartCoroutine(decalList[0].Co_ActiveDecal(8, 3));

        transform.position = decalList[0].transform.position - Vector3.up * 0.01f;
        decalList[(int)EDecalNumber.Jump].InActiveDecal(transform);

        animator.SetTrigger("JumpEnd");
    }
    private IEnumerator Co_Behavior_Move()
    {
        Debug.Log("이동");
        while (Vector3.Distance(transform.position, InGameManager.Instance.Player.transform.position) > 4)
        {
            animator.SetBool(ConstDefine.BOOL_ISMOVE, Move());
            yield return null;
        }
        animator.SetBool(ConstDefine.BOOL_ISMOVE, false);
    }
    protected override bool Move()
    {
        transform.forward = (InGameManager.Instance.Player.transform.position - transform.position).normalized; //플레이어를 바라보는 방향 벡터 구함
        transform.position += transform.forward.normalized * currentSpeed * Time.deltaTime; //대각선 이동을 위한 벡터 정규화 및 이동속도만큼 이동.
        return true;
    }
    public virtual void SummonStone(Vector3 pos)
    {
        if (stoneQueue.Count == 0) InitStoneQueue();
        stoneRef = stoneQueue.Dequeue();

        if (bReturnStone)
        {
            spareStoneList.Add(stoneRef);
        }
        else
        {
            stoneList.Add(stoneRef);
        }
        stoneSummonPos = pos;
        stoneSummonPos.y = summonedStonePosY;

        stoneRef.transform.SetParent(null);
        stoneRef.transform.position = stoneSummonPos;
        stoneRef.gameObject.SetActive(true);
        stoneRef.ResetStatus();
    }
    public void ReturnStone(CRedGolemStone redGolemStone, int stoneLevel)
    {
        redGolemStone.gameObject.SetActive(false);
        redGolemStone.transform.SetParent(transform);
        redGolemStone.transform.localPosition = Vector3.zero;
        redGolemStone.transform.localRotation = Quaternion.identity;

        if(stoneList.Contains(redGolemStone)) stoneList.Remove(redGolemStone);
        if (spareStoneList.Contains(redGolemStone)) spareStoneList.Remove(redGolemStone);

        ReturnStoneByLevel(redGolemStone, stoneLevel);
    }
    protected virtual void ReturnStoneByLevel(CRedGolemStone redGolemStone, int stoneLevel)
    {
        if(stoneLevel == 0)
        {
            stoneQueue.Enqueue(redGolemStone);
        }
    }
    public void AnimEvent_JumpAttack()
    {
        int num = Physics.OverlapSphereNonAlloc(transform.position, 5, jumpAttackCollisionArray, ConstDefine.LAYER_PLAYER);
        AttackInRangeUtility.AttackLayerInRange(jumpAttackCollisionArray, 20, num);
    }
    public void AnimEvent_JumpStart()
    {
        modelObject.SetActive(false);
    }
    public void AnimEvent_JumpEnd()
    {
        modelObject.SetActive(true);
        gameObject.layer = LayerMask.NameToLayer("Monster");
    }
    public virtual void AnimEvent_ThrowStone()
    {
        Projectile p = rangedAttackUtility.SummonProjectile(0.5f);
        p.SetShotDirection(transform.forward);
        p.SetDistance(12);
        p.ShotProjectile();
    }
    public virtual void AnimEvent_SummonStone()
    {
        int num = Physics.OverlapSphereNonAlloc(decalList[(int)EDecalNumber.SummonStone].transform.position, 1, summonStoneCollisionArray, ConstDefine.LAYER_PLAYER);
        AttackInRangeUtility.AttackLayerInRange(summonStoneCollisionArray, 10, num);
        SummonStone(decalList[(int)EDecalNumber.SummonStone].transform.position);
        decalList[(int)EDecalNumber.SummonStone].InActiveDecal(transform);
    }
}
