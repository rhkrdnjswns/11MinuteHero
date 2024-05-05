using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShadowKnife : ProjectileKnife
{
    private float secondSensingRadius;
    private bool isReturn;

    private System.Action SetIsReturn;
    public override void SetAction(System.Action action)
    {
        SetIsReturn += action;
    }
    public override void SetSensingRadius(float radius)
    {
        secondSensingRadius = radius;
    }
    protected override IEnumerator Co_Shot()
    {
        while (true)
        {
            if (bFinding) yield return finding;
            transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, target.position + Vector3.up * 0.5f) < 0.5f)
            {
                if(isReturn)
                {
                    owner.ReturnProjectile(this);
                    isReturn = false;
                    SetIsReturn();
                    yield break;
                }
                if (target.TryGetComponent(out Monster c))
                {
                    if(!c.IsDie)
                    {
                        c.Hit(damage);
#if UNITY_EDITOR
                        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
                    }
                }
                FindNewTarget();
            }
            yield return null;
        }
    }
    protected override void FindNewTarget()
    {
        bFinding = true;
        int num = Physics.OverlapSphereNonAlloc(transform.position, sensingRadius, newTargetCollisionArray, ConstDefine.LAYER_MONSTER);
        if (num == 0 || num == 1 && newTargetCollisionArray[0].transform == target)
        {
            int numNearPlayer = Physics.OverlapSphereNonAlloc(InGameManager.Instance.Player.transform.position, secondSensingRadius, newTargetCollisionArray, ConstDefine.LAYER_MONSTER);
            if(numNearPlayer == 0 || numNearPlayer == 1 && newTargetCollisionArray[0].transform == target)
            {
                target = InGameManager.Instance.Player.transform;
                isReturn = true;
                bFinding = false;
                Debug.Log("�÷��̾����� ���ư�");
                return;
            }
            num = numNearPlayer;
        }
        // Debug.Log("���� Ÿ�� : " + target.name);
        for (int i = 0; i < num; i++) //���� ������ ���� Ÿ���̾��� ���� ����
        {
            if (newTargetCollisionArray[i].transform == target) //���� Ÿ���̾��� ���� �ε����� �迭 �� �� ���� �־���
            {
                // Debug.Log("�ߺ� Ÿ�� : " + InRangeArray[i].name + ", ��ü�� Ÿ�� : " + InRangeArray[InRangeArray.Length - 1].name);
                newTargetCollisionArray[i] = newTargetCollisionArray[num - 1];
                //   Debug.Log("���ŵ� Ÿ�� : " + InRangeArray[i].name);
                break;
            }
        }

        int index = Random.Range(0, num - 1); //0 ~ �� �� - 1 ��ŭ�� �����ϰ� �����ؼ� �� �� ���� �������� �� ���� ���� �ʰ� ó��
        target = newTargetCollisionArray[index].transform;
        // Debug.Log("���� ���õ� Ÿ�� : " + target.name);
        bFinding = false;
    }
}
