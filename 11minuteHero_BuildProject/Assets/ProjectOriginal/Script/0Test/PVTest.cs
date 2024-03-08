using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PVTest : MonoBehaviour
{
    public List<PV> selectedChoiceList = new List<PV>();
    public List<PV> unPossessionGimmickList = new List<PV>();
    public List<PV> inPossessionList = new List<PV>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MixSkillOptions();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSkill(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSkill(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSkill(2);
        }
    }
    private void SelectSkill(int index)
    {
        if (selectedChoiceList.Count == 0) return;
        if (inPossessionList.Contains(selectedChoiceList[index]))
        {
            selectedChoiceList[index].LevelUp();
        }
        else
        {
            inPossessionList.Add(selectedChoiceList[index]);
            unPossessionGimmickList.Remove(selectedChoiceList[index]);
            selectedChoiceList[index].Init();

        }
        MixSkillOptions();
    }
    private void MixSkillOptions()
    {
        selectedChoiceList.Clear(); //������ �ʱ�ȭ

        List<int> unPossessionIndexList = new List<int>(); //���������� ���� ��ų ����Ʈ�� �ε����� ������
        for (int i = 0; i < unPossessionGimmickList.Count; i++)
        {
            unPossessionIndexList.Add(i); 
        }

        int rand = -1;
        if (inPossessionList.Count == 0) //�������� ��ų�� ���� ���
        {
            for (int i = 0; i < 3; i++) //������ ������ŭ �ݺ�
            {
                //���������� ���� ��ų �� �ϳ��� �ε����� ����
                rand = unPossessionIndexList[Random.Range(0, unPossessionIndexList.Count)];
                //���������� ���� ��ų ����Ʈ���� ���õ� �ε����� ��ų�� �������� �߰�
                selectedChoiceList.Add(unPossessionGimmickList[rand]);
                //���������� ���� ��ų �ε��� ����Ʈ���� ��� ���õ� �ε��� ���� (�ߺ� ������ ���� ó��)
                unPossessionIndexList.Remove(rand);
            }
            return;
        }

        List<int> inPossessionIndexList = new List<int>(); //�������� ��ų ����Ʈ�� �ε����� ������
        for (int i = 0; i < inPossessionList.Count; i++)
        {
            if(inPossessionList[i].Level < ConstDefine.SKILL_MAX_LEVEL)
            {
                inPossessionIndexList.Add(i);
            }
        }
        Debug.Log($"���� ��ų �� ��ȭ ������ ��ų�� ���� : {inPossessionIndexList.Count}");

        for (int i = 0; i < 3; i++) //������ ������ŭ �ݺ�
        {
            int weight = Random.Range(1, 101); // 1~100���� ����

            if (inPossessionIndexList.Count == 0) weight = 0; //�������� ��ų�� ��� �������� �߰��� ����� ó��

            if (weight < 41) //���������� ���� ��ų �������� �߰�
            {
                rand = unPossessionIndexList[Random.Range(0, unPossessionIndexList.Count)]; //35~39�ٰ� ������ ó��
                selectedChoiceList.Add(unPossessionGimmickList[rand]);
                unPossessionIndexList.Remove(rand);
            }
            else
            {
                rand = GetRandomIndexByLevel(inPossessionIndexList); //�������� ��ų �� �������� �������� ���� ��ų�� �ε��� ��ȯ
                selectedChoiceList.Add(inPossessionList[inPossessionIndexList[rand]]); //�������� �߰�
                inPossessionIndexList.Remove(inPossessionIndexList[rand]); //�������� ��ų �ε��� ����Ʈ���� ��� ��ȯ���� �ε��� ���� (�ߺ� ������ ���� ó��) 
            }
        }
    }
    private int GetRandomIndexByLevel(List<int> refList) //���� ��ų ����Ʈ�� ���� �ε��� ��ȯ
    {  
        List<int> levelList = new List<int>();
        int refIndex = 0;
        while(refIndex < refList.Count) //�������� �߰����� ���� ���� ��ų���� ������ �ߺ����� �ʰ� ������
        {
            if (levelList.Contains(inPossessionList[refList[refIndex]].Level)) refIndex++;
            else
            {
                levelList.Add(inPossessionList[refList[refIndex]].Level);
                refIndex++;
            }
        }
        //���� ��ų���� �������� ������������ ������ (����ġ ����� ����)
        levelList = levelList.OrderBy(o => o).ToList();
        int totalLevel = levelList.Sum(); //����ġ ������ ����

        int index = -1; //���������� ��ȯ�� ��

        if (levelList.Count == 1) //���� ��ų���� ������ ���� ���� ���
        {
            index = GetRandomIndexInSameLevelGroup(refList, totalLevel); //���� �ε��� ��ȯ
            return index;
        }

        int rand = Random.Range(0, totalLevel); //���� ��ų���� ������ �ٸ� ��� ���� ����
        int total = 0; //����ġ�� ���س��� �� (������ ����ġ = ����)

        for (int i = 0; i < levelList.Count; i++)
        {
            total += levelList[i]; // ������ ����ġ�� ������

            if (rand < total) //������ ���� ����ġ ���� ���ϴ� ��� 
            {
                index = GetRandomIndexInSameLevelGroup(refList, levelList[i]); //���� �ε��� ��ȯ
                break;
            }
        }
        return index;
    }
    private int GetRandomIndexInSameLevelGroup(List<int> refList, int level) //���õ� ������ ���� �ε��� �� ������ �ε��� ��ȯ
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < refList.Count; i++) //���� ��ų���� �ε��� ����Ʈ ��ȸ
        {
            if (inPossessionList[refList[i]].Level == level) //���õ� ������ ��ġ�ϴ� �ε��� ��Ҹ� �߰�
            {
                indexList.Add(i);
            }
        }
        return indexList[Random.Range(0, indexList.Count)]; //���õ� �ε��� �� �� ������ �� ��ȯ
    }

    #region �Ⱦ�
    //private int GetIndex(List<int> refList)
    //{
    //    List<int> levelList = new List<int>(3);
    //    for (int i = 0; i < inPossessionList.Count; i++)
    //    {
    //        if (inPossessionList[i].Level > 3) continue;
    //        levelList[inPossessionList[i].Level - 1]++;
    //    }
    //    int totalLevel = levelList.Sum();
    //    Debug.Log("LevelList.Count = " + levelList.Count);

    //    int index = -1;
    //    if(levelList.Count == 1)
    //    {
    //        index = GetSameLevel(refList, totalLevel);
    //        return index;
    //    }

    //    int rand = Random.Range(0, totalLevel);
    //    int total = 0;
    //    for (int i = 0; i < levelList.Count; i++)
    //    {
    //        total += levelList[i];
    //        if (rand < total)
    //        {
    //            index = GetSameLevel(refList, levelList[i]);
    //            break;
    //        }
    //    }
    //    return index;
    //}
    //private int GetRandomIndexInPossessionSkillList()
    //{
    //    List<int> uniqueLevelList = inPossessionList.Select(s => s.Level).Where(w => w < 4).Distinct().ToList();
    //    int totalLevel = uniqueLevelList.Sum();

    //    int index = -1;

    //    if (uniqueLevelList.Count == 1)
    //    {
    //        index = GetRandomIndexSameLevelGroup(totalLevel);
    //        return index;
    //    }

    //    int rand = Random.Range(0, totalLevel);
    //    int total = 0;
    //    for (int i = 0; i < uniqueLevelList.Count; i++)
    //    {
    //        total += uniqueLevelList[i];
    //        if (rand <= total)
    //        {
    //            index = GetRandomIndexSameLevelGroup(uniqueLevelList[i]);
    //            break;
    //        }
    //    }
    //    return index;
    //}
    //private int GetRandomIndexSameLevelGroup(int level)
    //{
    //    List<int> indexList = new List<int>();
    //    for (int i = 0; i < inPossessionList.Count; i++)
    //    {
    //        if (inPossessionList[i].Level == level)
    //        {
    //            indexList.Add(i);
    //        }
    //    }
    //    return indexList[Random.Range(0, indexList.Count)];
    //}
    #endregion
}
public enum EType
{
    Passive = 0,
    Active
}

[System.Serializable]
public class PV
{
    public EType eType;
    public int Level;
    public string name;

    public void LevelUp()
    {
        Level++;
    }
    public void Init()
    {
        Debug.Log(name + " ȹ��");
        LevelUp();
    }
}

