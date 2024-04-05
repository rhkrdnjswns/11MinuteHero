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
    public void AttackLayerInSquare(Collider[] inSquareArray, float damage) //반경 내 Character 오브젝트에 피해를 줌
    {
        if (inSquareArray.Length == 0) return;

        foreach (var item in inSquareArray)
        {
            item.GetComponent<Character>().Hit(damage);
        }
    }
}
