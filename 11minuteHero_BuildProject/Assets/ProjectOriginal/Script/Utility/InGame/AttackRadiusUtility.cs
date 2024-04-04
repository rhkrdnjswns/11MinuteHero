using UnityEngine;
using System.Linq;

public enum EAttackType
{
    Slow,
    Sturn
}
[System.Serializable]
public class AttackRadiusUtility //�ݰ� ������ Ư�� ���̾ �˻��ϴ� ����� ���� Ŭ���� 
{
    [SerializeField] private float radius; //�ݰ�
    [SerializeField] private LayerMask layerMask; //������ ���̾�
    
    public float Radius { get => radius; set => radius = value; }
    public Collider[] GetLayerInRadius(Transform transform) //transform.position���� �ݰ游ŭ ���� �׷� ����� �浹ü �迭�� ��ȯ
    {
        return Physics.OverlapSphere(transform.position, radius, layerMask);
    }
    public Collider[] GetLayerInRadius(Transform transform, float radius) //transform.position���� �ݰ游ŭ ���� �׷� ����� �浹ü �迭�� ��ȯ
    {
        return Physics.OverlapSphere(transform.position, radius, layerMask);
    }
    public Collider[] GetLayerInRadiusSortedByDistance(Transform transform)//transform.position���� �ݰ游ŭ ���� �׷� ����� �浹ü�� ����� ������ �����ؼ� �迭�� ��ȯ
    {
        return Physics.OverlapSphere(transform.root.position, radius, layerMask).OrderBy(o => Vector3.SqrMagnitude(o.transform.position - transform.position)).ToArray();
    }
    public void AttackLayerInRadius(Collider[] inRadiusArray, float damage) //�ݰ� �� Character ������Ʈ�� ���ظ� ��
    {    
        if (inRadiusArray.Length == 0) return;

        foreach (var item in inRadiusArray)
        {
            item.GetComponent<Character>().Hit(damage);
        }      
    }
    public void AttackLayerInRadius(Collider[] inRadiusArray, float damage, EAttackType eAttackType, float duration, float percentage = 0) //�ݰ� �� Character ������Ʈ�� ���ؿ� ����� ����
    {
        if (inRadiusArray.Length == 0) return;

        foreach (var item in inRadiusArray)
        {
            item.GetComponent<Character>().Hit(damage);
            Monster m = item.GetComponent<Monster>();
            if(eAttackType == EAttackType.Slow)
            {
                m.DebuffList.Add(new DSlowDown(EDebuffType.Slow, duration, m, m.Speed * percentage / 100));
            }
            else if(eAttackType == EAttackType.Sturn)
            {
                m.DebuffList.Add(new DSturn(EDebuffType.Slow, duration, m));
            }
        }
    }
    public int GetKillCountAttackLayerInRadius(Collider[] inRadiusArray, float damage)
    {
        if (inRadiusArray.Length == 0) return 0;

        Character c;
        int count = 0;
        foreach (var item in inRadiusArray)
        {
            c = item.GetComponent<Character>();
            c.Hit(damage);
            if (c.IsDie) count++;
        }
        return count;
    }
}
