using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRedGolemNormal : BRedGolem
{
    private Transform[] decalParentArray = new Transform[2];
    [SerializeField] private GameObject indestructibleStonePrefab;
    protected Queue<CRedGolemStone> indestructibleStoneQueue = new Queue<CRedGolemStone>();

    protected float summonedIndestructibleStonePosY;
    protected override void InitStoneQueue()
    {
        base.InitStoneQueue();
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = Instantiate(indestructibleStonePrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;

            indestructibleStoneQueue.Enqueue(obj.GetComponent<CRedGolemStone>().SetReference(transform));
        }
    }
    public override void InitBoss()
    {
        base.InitBoss();
        for (int i = 0; i < decalParentArray.Length; i++)
        {
            decalParentArray[i] = decalList[i + (int)EDecalNumber.SummonStoneX].transform.parent;
        }
    }
    protected override void SetSummonedStonePosY()
    {
        base.SetSummonedStonePosY();
        summonedIndestructibleStonePosY = 0f;
    }
    public override void AnimEvent_ThrowStone()
    {
        for (int i = -1; i < 2; i++)
        {
            Projectile p = rangedAttackUtility.SummonProjectile(0.5f);
            Vector3 direction = Quaternion.Euler(0, 25 * i, 0) * transform.forward;
            p.SetShotDirection(direction);
            p.SetDistance(12);
            p.ShotProjectile();
        }
    }
    protected override IEnumerator Co_Behavior_SummonStone()
    {
        Debug.Log("µ¹ ¼ÒÈ¯");
        transform.LookAt(InGameManager.Instance.Player.transform);
        animator.SetTrigger("SummonStone");

        for(int i = 0; i < decalParentArray.Length; i++)
        {
            decalParentArray[i].transform.SetParent(null);
            decalParentArray[i].transform.position = InGameManager.Instance.Player.transform.position;
            if (i == 0)
            {
                decalParentArray[i].forward = InGameManager.Instance.Player.transform.forward;
            }
            else
            {
                decalParentArray[i].forward = InGameManager.Instance.Player.transform.right;
            }
            StartCoroutine(decalList[i + (int)EDecalNumber.SummonStoneX].Co_ActiveDecal(new Vector3(0.8f, 5, 1)));
        }

        yield return null;
    }
    public override void AnimEvent_SummonStone()
    {
        for(int i = 0; i < decalParentArray.Length; i++)
        {
            attackInSquareUtility.AttackLayerInSquare(attackInSquareUtility.GetLayerInSquare(decalList[(int)EDecalNumber.SummonStoneX + i].transform.position, new Vector3(0.4f, 1, 2.5f), decalParentArray[i].rotation), 15);

            decalList[(int)EDecalNumber.SummonStoneX + i].InActiveDecal(decalParentArray[i]);
            decalParentArray[i].transform.SetParent(transform);
        }
        SummonStone(decalList[(int)EDecalNumber.SummonStoneX].transform.position);
    }
    public override void SummonStone(Vector3 pos)
    {
        int rand = Random.Range(1, 101);
        float posY;

        if (rand <= 70)
        {
            if (stoneQueue.Count == 0) InitStoneQueue();
            stoneRef = stoneQueue.Dequeue();
            posY = summonedStonePosY;
        }
        else
        {
            if (indestructibleStoneQueue.Count == 0) InitStoneQueue();
            stoneRef = indestructibleStoneQueue.Dequeue();
            posY = summonedIndestructibleStonePosY;
        }

        if (bReturnStone)
        {
            spareStoneList.Add(stoneRef);
        }
        else
        {
            stoneList.Add(stoneRef);
        }
        stoneSummonPos = pos;
        stoneSummonPos.y = posY;

        stoneRef.transform.SetParent(null);
        stoneRef.transform.position = stoneSummonPos;
        stoneRef.gameObject.SetActive(true);
        stoneRef.ResetStatus();
    }
    protected override void ReturnStoneByLevel(CRedGolemStone redGolemStone, int stoneLevel)
    {
        base.ReturnStoneByLevel(redGolemStone, stoneLevel);
        if(stoneLevel == 1)
        {
            indestructibleStoneQueue.Enqueue(redGolemStone);
        }
    }
}
