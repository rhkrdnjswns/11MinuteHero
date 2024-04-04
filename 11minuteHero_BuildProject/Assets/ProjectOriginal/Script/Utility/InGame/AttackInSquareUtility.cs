using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackInSquareUtility
{
    [SerializeField] private LayerMask layerMask;
    public Collider[] GetLayerInSquare(Vector3 center, Vector3 halfExtents, Quaternion rot)
    {
        return Physics.OverlapBox(center, halfExtents, rot, layerMask);
    }
    public void AttackLayerInSquare(Collider[] inSquareArray, float damage) //�ݰ� �� Character ������Ʈ�� ���ظ� ��
    {
        if (inSquareArray.Length == 0) return;

        foreach (var item in inSquareArray)
        {
            item.GetComponent<Character>().Hit(damage);
        }
    }
    public void AttackAndKnockbackLayerInSquare(Collider[] inSquareArray, float damage, float speed, float duration, Vector3 direction) //�ݰ� �� Character ������Ʈ�� ���ظ� ��
    {
        if (inSquareArray.Length == 0) return;

        foreach (var item in inSquareArray)
        {
            Character c = item.GetComponent<Character>();
            if (c == null) continue;
            c.Hit(damage);
            c.KnockBack(speed, duration, direction);
        }
    }
}
