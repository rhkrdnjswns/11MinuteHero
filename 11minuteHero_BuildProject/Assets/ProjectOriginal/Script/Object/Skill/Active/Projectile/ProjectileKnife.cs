using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileKnife : Projectile
{
    protected float bounceCount;
    protected float currentCount;
    protected float sensingRadius;

    protected Transform target;
    protected bool bFinding;

    protected WaitUntil finding;

    protected Collider[] newTargetCollisionArray = new Collider[50];
    public override void SetBounceCount(int count)
    {
        bounceCount = count;
    }
    public override void SetTargetTransform(Transform transform)
    {
        target = transform;
        this.transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
    }
    public override void SetAttackRadius(float radius) //attackRadius�� �������� ���� (��ȭ��ų�� �� ��° ���������� �־)
    {
        sensingRadius = radius;
    }
    protected override IEnumerator Co_Shot()
    {
        currentCount = bounceCount;
        while(true)
        {
            if(bFinding) yield return finding;

            transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
            transform.position += transform.forward * speed * Time.deltaTime;

            if(Vector3.Distance(transform.position, target.position + Vector3.up * 0.5f) < 0.5f)
            {
                if (target.TryGetComponent(out Character c))
                {
                    if(!c.IsDie)
                    {
                        c.Hit(damage);
#if UNITY_EDITOR
                        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
                    }
                }

                if (currentCount <= 0) owner.ReturnProjectile(this);

                FindNewTarget();
                currentCount--;
            }
            yield return null;
        }
    }
    protected virtual void FindNewTarget()
    {
        bFinding = true;
        int num = Physics.OverlapSphereNonAlloc(transform.position, sensingRadius, newTargetCollisionArray, ConstDefine.LAYER_MONSTER);
        if(num == 0 || num == 1 && newTargetCollisionArray[0].transform == target)
        {
            bFinding = false;
            owner.ReturnProjectile(this);
            return;
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
