using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionChildID : MonoBehaviour
{
    [SerializeField] private ESkillActiveID[] childIDArray;

    public int[] GetChildID()
    {
        int[] array = new int[childIDArray.Length];
        for (int i = 0; i < childIDArray.Length; i++)
        {
            array[i] = (int)childIDArray[i];
        }
        return array;
    }
}
