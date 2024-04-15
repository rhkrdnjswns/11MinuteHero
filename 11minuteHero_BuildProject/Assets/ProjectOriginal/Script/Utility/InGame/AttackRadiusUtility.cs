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

    private Character character;
    private NormalMonster monster;
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
            character = item.GetComponent<Character>();
            character?.Hit(damage);
        }      
    }
    public void AttackLayerInRadius(Collider[] inRadiusArray, float damage, EAttackType eAttackType, float duration, float percentage = 0) //�ݰ� �� Character ������Ʈ�� ���ؿ� ����� ����
    {
        if (inRadiusArray.Length == 0) return;

        foreach (var item in inRadiusArray)
        {
            character = item.GetComponent<Character>();
            character?.Hit(damage);

            monster = item.GetComponent<NormalMonster>();
            if(monster != null) 
            {
                if (eAttackType == EAttackType.Slow) //����� ���� �κ��� �� �ٲ���ҵ�(�ν��Ͻ� ���� ���ϴ� ��������)
                {
                    monster.DebuffList.Add(new DSlowDown(EDebuffType.Slow, duration, monster, monster.Speed * percentage / 100));
                }
                else if (eAttackType == EAttackType.Sturn)
                {
                    monster.DebuffList.Add(new DSturn(EDebuffType.Slow, duration, monster));
                }
            }
        }
    }
    public int GetKillCountAttackLayerInRadius(Collider[] inRadiusArray, float damage)
    {
        if (inRadiusArray.Length == 0) return 0;

        int count = 0;
        foreach (var item in inRadiusArray)
        {
            character = item.GetComponent<Character>();
            if(character != null)
            {
                character.Hit(damage);
                if (character.IsDie) count++;
            }
        }
        return count;
    }
}
