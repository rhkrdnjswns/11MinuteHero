using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRedGolemNormal : BRedGolem
{
    private Transform[] decalParentArray = new Transform[2];
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
        summonedStonePosY = 0f;
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
    protected override IEnumerator Co_SummonStone()
    {
        Debug.Log("µ¹ ¼ÒÈ¯");
        transform.LookAt(InGameManager.Instance.Player.transform);
        animator.SetTrigger("SummonStone");

        for(int i = 0; i < decalParentArray.Length; i++)
        {
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
        attackInSquareUtility.AttackLayerInSquare(attackInSquareUtility.GetLayerInSquare(decalList[(int)EDecalNumber.SummonStoneX].transform.position, new Vector3(0.4f, 1, 2.5f), transform.rotation), 15);
        attackInSquareUtility.AttackLayerInSquare(attackInSquareUtility.GetLayerInSquare(decalList[(int)EDecalNumber.SummonStoneZ].transform.position, new Vector3(0.4f, 1, 2.5f), transform.rotation), 15);
       
        SummonStone(decalList[(int)EDecalNumber.SummonStoneX].transform.position);

        decalList[(int)EDecalNumber.SummonStoneX].InActiveDecal(decalParentArray[0]);
        decalList[(int)EDecalNumber.SummonStoneZ].InActiveDecal(decalParentArray[1]);
    }
}
