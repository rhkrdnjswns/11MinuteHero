using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKnife : Projectile
{
    protected float bounceCount;
    protected float currentCount;
    protected AttackRadiusUtility attackRadiusUtility; //��ź ���� �ݰ� ����
    protected Transform target;
    protected bool bFinding;
    public override void SetCount(int count)
    {
        bounceCount = count;
    }
    public override void SetAttackRadiusUtility(AttackRadiusUtility attackRadiusUtility)
    {
        this.attackRadiusUtility = attackRadiusUtility;
    }
    public override void ShotProjectile(Transform target)
    {
        currentCount = bounceCount;
        this.target = target;
        transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
        base.ShotProjectile();
    }
    protected override IEnumerator Co_Shot()
    {
        while(true)
        {
            if (bFinding) yield return new WaitUntil(() => !bFinding);
            transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
            transform.position += transform.forward * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            if(Vector3.Distance(transform.position, target.position + Vector3.up * 0.5f) < 0.5f)
            {
                if (!target.GetComponent<Character>().IsDie)
                {
                    target.GetComponent<Character>().Hit(rangedAttackUtility.ProjectileDamage);
                }
                if (currentCount <= 0) rangedAttackUtility.ReturnProjectile(this);
                FindNewTarget();
                currentCount--;
            }
            yield return null;
        }
    }
    protected override void OnTriggerEnter(Collider other) //����ü �浹 ó��
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER)) //���Ϳ� �ε��� ���
        {
            other.GetComponent<Character>().Hit(rangedAttackUtility.ProjectileDamage); //Monster Ŭ������ �����Ͽ� ������ ����
            if (currentCount < 0) rangedAttackUtility.ReturnProjectile(this);
            else
            {
                FindNewTarget();
                currentCount--;
            }
        }
    }
    protected virtual void FindNewTarget()
    {
        bFinding = true;
        Collider[] InRangeArray = attackRadiusUtility.GetLayerInRadius(transform);
        if(InRangeArray.Length == 0 || InRangeArray.Length == 1 && InRangeArray[0].transform == target)
        {
            bFinding = false;
            rangedAttackUtility.ReturnProjectile(this);
            return;
        }
       // Debug.Log("���� Ÿ�� : " + target.name);
        for (int i = 0; i < InRangeArray.Length; i++) //���� ������ ���� Ÿ���̾��� ���� ����
        {
            if (InRangeArray[i].transform == target) //���� Ÿ���̾��� ���� �ε����� �迭 �� �� ���� �־���
            {
               // Debug.Log("�ߺ� Ÿ�� : " + InRangeArray[i].name + ", ��ü�� Ÿ�� : " + InRangeArray[InRangeArray.Length - 1].name);
                InRangeArray[i] = InRangeArray[InRangeArray.Length - 1];
             //   Debug.Log("���ŵ� Ÿ�� : " + InRangeArray[i].name);
                break;
            }
        }

        int index = Random.Range(0, InRangeArray.Length - 1); //0 ~ �� �� - 1 ��ŭ�� �����ϰ� �����ؼ� �� �� ���� �������� �� ���� ���� �ʰ� ó��
        target = InRangeArray[index].transform;
       // Debug.Log("���� ���õ� Ÿ�� : " + target.name);
        bFinding = false;
    }
}
