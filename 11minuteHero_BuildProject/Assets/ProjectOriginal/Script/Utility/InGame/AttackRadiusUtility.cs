using UnityEngine;
using System.Linq;

public enum EAttackType
{
    Slow,
    Sturn
}
[System.Serializable]
public class AttackRadiusUtility //반경 내에서 특정 레이어를 검사하는 기능을 가진 클래스 
{
    [SerializeField] private float radius; //반경
    [SerializeField] private LayerMask layerMask; //검출할 레이어
    
    public float Radius { get => radius; set => radius = value; }
    public Collider[] GetLayerInRadius(Transform transform) //transform.position에서 반경만큼 원을 그려 검출된 충돌체 배열로 반환
    {
        return Physics.OverlapSphere(transform.position, radius, layerMask);
    }
    public Collider[] GetLayerInRadius(Transform transform, float radius) //transform.position에서 반경만큼 원을 그려 검출된 충돌체 배열로 반환
    {
        return Physics.OverlapSphere(transform.position, radius, layerMask);
    }
    public Collider[] GetLayerInRadiusSortedByDistance(Transform transform)//transform.position에서 반경만큼 원을 그려 검출된 충돌체를 가까운 순서로 정렬해서 배열로 반환
    {
        return Physics.OverlapSphere(transform.root.position, radius, layerMask).OrderBy(o => Vector3.SqrMagnitude(o.transform.position - transform.position)).ToArray();
    }
    public void AttackLayerInRadius(Collider[] inRadiusArray, float damage) //반경 내 Character 오브젝트에 피해를 줌
    {    
        if (inRadiusArray.Length == 0) return;

        foreach (var item in inRadiusArray)
        {
            item.GetComponent<Character>().Hit(damage);
        }      
    }
    public void AttackLayerInRadius(Collider[] inRadiusArray, float damage, EAttackType eAttackType, float duration, float percentage = 0) //반경 내 Character 오브젝트에 피해와 디버프 적용
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
