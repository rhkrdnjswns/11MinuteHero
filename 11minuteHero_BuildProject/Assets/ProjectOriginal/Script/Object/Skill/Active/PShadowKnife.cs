using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShadowKnife : PKnife
{
    private AttackRadiusUtility secondRadiusUtility;
    private KShadowKnife kShadowKnife;
    private bool isReturn;
    public void SetShadowKnifeReference(AttackRadiusUtility attackRadiusUtility, KShadowKnife kShadowKnife)
    {
        secondRadiusUtility = attackRadiusUtility;
        this.kShadowKnife = kShadowKnife;
    }
    protected override IEnumerator Co_Shot()
    {
        while (true)
        {
            if (bFinding) yield return finding;
            transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
            transform.position += transform.forward * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, target.position + Vector3.up * 0.5f) < 0.5f)
            {
                if(isReturn)
                {
                    rangedAttackUtility.ReturnProjectile(this);
                    isReturn = false;
                    kShadowKnife.IsReturn = true;
                    yield break;
                }
                if (!target.GetComponent<Character>().IsDie)
                {
                    target.GetComponent<Character>().Hit(rangedAttackUtility.ProjectileDamage);
#if UNITY_EDITOR
                    InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += rangedAttackUtility.ProjectileDamage;
#endif

                }
                FindNewTarget();
            }
            yield return null;
        }
    }
    protected override void FindNewTarget()
    {
        bFinding = true;
        Collider[] InRangeArray = attackRadiusUtility.GetLayerInRadius(transform);
        if (InRangeArray.Length == 0 || InRangeArray.Length == 1 && InRangeArray[0].transform == target)
        {
            InRangeArray = secondRadiusUtility.GetLayerInRadius(InGameManager.Instance.Player.transform);
            if(InRangeArray.Length == 0 || InRangeArray.Length == 1 && InRangeArray[0].transform == target)
            {
                target = InGameManager.Instance.Player.transform;
                isReturn = true;
                bFinding = false;
                return;
            }
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
