using UnityEngine;
using System;

public class AttackInRangeUtility //�ݰ� ������ Ư�� ���̾ �˻��ϴ� ����� ���� Ŭ���� 
{
    public static void AttackLayerInRange(Collider[] inRangeArray, float damage, int max) //�ݰ� �� Character ������Ʈ�� ���ظ� ��
    {    
        if (max == 0) return;

        for (int i = 0; i < max; i++)
        {
            if(inRangeArray[i].TryGetComponent(out Character c))
            {
                c.Hit(damage);
            }
        }   
    }
    public static void AttackAndSlowDownLayerInRange(Collider[] inRangeArray, float damage, int max, float value, float duration) //�ݰ� �� Character ������Ʈ�� ���ظ� �ְ� ���ο�
    {
        if (max == 0) return;

        for (int i = 0; i < max; i++)
        {
            if(inRangeArray[i].TryGetComponent(out Character c))
            {
                c.Hit(damage);
                c.GetComponent<IDebuffApplicable>()?.SlowDown(value, duration);
            }
        }
    }
    public static void AttackAndStunLayerInRange(Collider[] inRangeArray, float damage, int max, float duration) //�ݰ� �� Character ������Ʈ�� ���ظ� �ְ� ���ο�
    {
        if (max == 0) return;

        for (int i = 0; i < max; i++)
        {
            if (inRangeArray[i].TryGetComponent(out Character c))
            {
                c.Hit(damage);
                c.GetComponent<IDebuffApplicable>()?.Stun(duration);
            }
        }
    }
    public static int GetKillCountAttackLayerInRadius(Collider[] inRangeArray, float damage, int max)
    {
        if (max == 0) return 0;

        int count = 0;
        for (int i = 0; i < max; i++)
        {
            if(inRangeArray[i].TryGetComponent(out Character c))
            {
                c.Hit(damage);
                if (c.IsDie) count++;
            }
        }
        return count;
    }
    public static void QuickSortCollisionArray(Collider[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(arr, left, right);

            QuickSortCollisionArray(arr, left, pivotIndex - 1);
            QuickSortCollisionArray(arr, pivotIndex + 1, right);
        }
    }
    private static int Partition(Collider[] arr, int left, int right)
    {
        Collider pivotValue = arr[right];
        int pivotIndex = left;

        for (int i = left; i < right; i++)
        {
            if (Vector3.SqrMagnitude(arr[i].transform.position - InGameManager.Instance.Player.transform.position) < 
                Vector3.SqrMagnitude(pivotValue.transform.position - InGameManager.Instance.Player.transform.position))
            {
                Swap(arr, pivotIndex, i);
                pivotIndex++;
            }
        }

        Swap(arr, pivotIndex, right);
        return pivotIndex;
    }

    private static void Swap(Collider[] arr, int a, int b)
    {
        Collider temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }
}

