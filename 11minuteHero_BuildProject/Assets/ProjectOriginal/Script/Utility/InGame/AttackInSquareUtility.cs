using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackInSquareUtility
{
    public static void AttackLayerInSquare(Collider[] inSquareArray, float damage, float num) //�ݰ� �� Character ������Ʈ�� ���ظ� ��
    {
        if (inSquareArray.Length == 0) return;

        foreach (var item in inSquareArray)
        {
            item.GetComponent<Character>()?.Hit(damage);
        }
    }
}
