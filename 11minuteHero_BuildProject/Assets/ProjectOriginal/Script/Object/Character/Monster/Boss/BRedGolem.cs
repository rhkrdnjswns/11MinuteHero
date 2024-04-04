using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRedGolem : Boss
{
    private enum EDecalNumber
    {
        Jump = 0,
        SummonStone,
        KnockbackPunch
    }
    private bool bAction;
    private bool bReturnStone;
    private GameObject modelObject;
    [SerializeField] private RangedAttackUtility rangedAttackUtility;
    [SerializeField] private AttackRadiusUtility jumpAttack;
    [SerializeField] private AttackInSquareUtility knockbackPunch;
    [SerializeField] private GameObject stonePrefab;

    private int returnStoneCount;
    private List<CRedGolemStone> stoneList = new List<CRedGolemStone>();
    private Queue<CRedGolemStone> stoneQueue = new Queue<CRedGolemStone>();
    public int ReturnStoneCount { get => returnStoneCount; set => returnStoneCount = value; }
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
        modelObject = transform.GetChild(0).gameObject;
        InitStoneQueue();
        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile();
        rangedAttackUtility.SetDamage(10);
    }
    private void InitStoneQueue()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = Instantiate(stonePrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;

            stoneQueue.Enqueue(obj.GetComponent<CRedGolemStone>().SetReference(transform).InitDamageUIContainer());
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
    private IEnumerator Co_ShortRangeBehavior()
    {
        int rand = Random.Range(1, 101);
        if (rand < 46)
        {
            yield return StartCoroutine(Co_KnockBackPunch());
        }
        else if (rand > 45 && rand < 76)
        {
            yield return StartCoroutine(Co_SummonStone());
        }
        else
        {
            yield return StartCoroutine(Co_ReturnStone());
        }
        yield return new WaitForSeconds(2f);

        bAction = false;
    }
    private IEnumerator Co_MiddleRangeBehavior()
    {
        int rand = Random.Range(1, 101);
        if (rand < 46)
        {
            yield return StartCoroutine(Co_ThrowStone());
        }
        else if (rand > 45 && rand < 76)
        {
            yield return StartCoroutine(Co_SummonStone());
        }
        else
        {
            yield return StartCoroutine(Co_ReturnStone());//StartCoroutine(Co_Jump());
        }
        yield return new WaitForSeconds(2f);

        bAction = false;
    }
    private IEnumerator Co_LongRangeBehavior()
    {
        int rand = Random.Range(1, 101);
        if (rand < 31)
        {
            yield return StartCoroutine(Co_Move());
        }
        else if (rand > 30 && rand < 71)
        {
            yield return StartCoroutine(Co_ReturnStone());
        }
        else
        {
            yield return StartCoroutine(Co_Jump());
        }
        yield return new WaitForSeconds(2f);

        bAction = false;
    }
    private IEnumerator Co_SpreadStone()
    {
        Debug.Log("돌 뿌리기");
        foreach (var item in stoneList)
        {
            item.StartCoroutine(item.Co_SpreadStone(Quaternion.Euler(0,Random.Range(0,361),0) * Vector3.forward, Random.Range(1f,2.6f)));
        }
        stoneList.Clear();
        yield return null;
    }
    private IEnumerator Co_SummonStone()
    {
        Debug.Log("돌 소환");
        transform.LookAt(InGameManager.Instance.Player.transform);
        animator.SetTrigger("SummonStone");

        decalList[(int)EDecalNumber.SummonStone].transform.SetParent(null);
        decalList[(int)EDecalNumber.SummonStone].transform.position = InGameManager.Instance.Player.transform.position;
        StartCoroutine(decalList[(int)EDecalNumber.SummonStone].Co_ActiveDecal(2));

        yield return null;
    }
    private IEnumerator Co_ReturnStone()
    {
        Debug.Log("돌 모으기");
        if (stoneList.Count == 0) yield break;
        bReturnStone = true;
        animator.SetTrigger("ReturnStone");
        animator.SetBool("IsEndReturn", false);
        foreach (var item in stoneList)
        {
            item.StartCoroutine(item.Co_CollectStone(2));
            yield return null;
        }
        yield return new WaitUntil(() => returnStoneCount == stoneList.Count);
        IncreaseHp(5 * returnStoneCount);

        if(returnStoneCount >= 3)
        {
            animator.SetBool("bSpreadStone", true);
            animator.SetBool("IsEndReturn", true);
            yield return StartCoroutine(Co_SpreadStone());
        }
        else
        {
            animator.SetBool("bSpreadStone", false);
            animator.SetBool("IsEndReturn", true);
            while (stoneList.Count != 0)
            {
                ReturnStone(stoneList[0]);
                yield return null;
            }
        }
        returnStoneCount = 0;
        bReturnStone = false;
    }
    private IEnumerator Co_KnockBackPunch()
    {
        Debug.Log("펀치");
        transform.LookAt(InGameManager.Instance.Player.transform);

        decalList[(int)EDecalNumber.KnockbackPunch].transform.SetParent(null);
        decalList[(int)EDecalNumber.KnockbackPunch].transform.position = transform.position + transform.forward * 2;
        yield return StartCoroutine(decalList[(int)EDecalNumber.KnockbackPunch].Co_ActiveDecal(new Vector3(4,6,1), 1f));
        decalList[(int)EDecalNumber.KnockbackPunch].InActiveDecal(transform);

        animator.SetTrigger("Punch");
        knockbackPunch.AttackAndKnockbackLayerInSquare(knockbackPunch.GetLayerInSquare(transform.position + transform.forward * 2, new Vector3(2, 1, 3), transform.rotation),
            10, 5f, 1f, transform.forward);
    }
    private IEnumerator Co_ThrowStone()
    {
        Debug.Log("돌 던지기");
        transform.LookAt(InGameManager.Instance.Player.transform);
        animator.SetTrigger("ThrowStone");

        yield return null;
    }
    private IEnumerator Co_Jump()
    {
        Debug.Log("점프");
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
    private IEnumerator Co_Move()
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
    public void SummonStone(Vector3 pos)
    {
        if (bReturnStone) return;
        if (stoneQueue.Count == 0) InitStoneQueue();
        CRedGolemStone obj = stoneQueue.Dequeue();
        stoneList.Add(obj);

        obj.ResetStatus();
        obj.transform.SetParent(null);
        obj.transform.position = pos;
        obj.gameObject.SetActive(true);
    }
    public void ReturnStone(CRedGolemStone redGolemStone)
    {
        redGolemStone.gameObject.SetActive(false);
        redGolemStone.transform.SetParent(transform);
        redGolemStone.transform.localPosition = Vector3.zero;
        redGolemStone.transform.localRotation = Quaternion.identity;

        if(stoneList.Contains(redGolemStone)) stoneList.Remove(redGolemStone);
        stoneQueue.Enqueue(redGolemStone);
    }
    public void AnimEvent_JumpAttack()
    {
        jumpAttack.AttackLayerInRadius(jumpAttack.GetLayerInRadius(transform), 20);
    }
    public void AnimEvent_JumpStart()
    {
        modelObject.SetActive(false);
    }
    public void AnimEvent_JumpEnd()
    {
        modelObject.SetActive(true);
    }
    public void AnimEvent_ThrowStone()
    {
        Projectile p = rangedAttackUtility.SummonProjectile(0.5f);
        p.SetShotDirection((InGameManager.Instance.Player.transform.position - transform.position).normalized);
        p.SetDistance(12);
        p.ShotProjectile();
    }
    public void AnimEvent_SummonStone()
    {
        jumpAttack.AttackLayerInRadius(jumpAttack.GetLayerInRadius(decalList[(int)EDecalNumber.SummonStone].transform, 1), 10);
        SummonStone(decalList[(int)EDecalNumber.SummonStone].transform.position + Vector3.up * 0.49f);
        decalList[(int)EDecalNumber.SummonStone].InActiveDecal(transform);
    }
}
