using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackInSquareUtility
{
    public static void AttackLayerInSquare(Collider[] inSquareArray, float damage, float num) //반경 내 Character 오브젝트에 피해를 줌
    {
        if (inSquareArray.Length == 0) return;

        foreach (var item in inSquareArray)
        {
            item.GetComponent<Character>()?.Hit(damage);
        }
    }
}
