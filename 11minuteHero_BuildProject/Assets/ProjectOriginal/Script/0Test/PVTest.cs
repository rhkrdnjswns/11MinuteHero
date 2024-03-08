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
        selectedChoiceList.Clear(); //선택지 초기화

        List<int> unPossessionIndexList = new List<int>(); //보유중이지 않은 스킬 리스트의 인덱스를 가져옴
        for (int i = 0; i < unPossessionGimmickList.Count; i++)
        {
            unPossessionIndexList.Add(i); 
        }

        int rand = -1;
        if (inPossessionList.Count == 0) //보유중인 스킬이 없는 경우
        {
            for (int i = 0; i < 3; i++) //선택지 개수만큼 반복
            {
                //보유중이지 않은 스킬 중 하나의 인덱스를 선택
                rand = unPossessionIndexList[Random.Range(0, unPossessionIndexList.Count)];
                //보유중이지 않은 스킬 리스트에서 선택된 인덱스의 스킬을 선택지에 추가
                selectedChoiceList.Add(unPossessionGimmickList[rand]);
                //보유중이지 않은 스킬 인덱스 리스트에서 방금 선택된 인덱스 제거 (중복 방지를 위한 처리)
                unPossessionIndexList.Remove(rand);
            }
            return;
        }

        List<int> inPossessionIndexList = new List<int>(); //보유중인 스킬 리스트의 인덱스를 가져옴
        for (int i = 0; i < inPossessionList.Count; i++)
        {
            if(inPossessionList[i].Level < ConstDefine.SKILL_MAX_LEVEL)
            {
                inPossessionIndexList.Add(i);
            }
        }
        Debug.Log($"보유 스킬 중 강화 가능한 스킬의 개수 : {inPossessionIndexList.Count}");

        for (int i = 0; i < 3; i++) //선택지 개수만큼 반복
        {
            int weight = Random.Range(1, 101); // 1~100까지 구함

            if (inPossessionIndexList.Count == 0) weight = 0; //보유중인 스킬이 모두 선택지에 추가된 경우의 처리

            if (weight < 41) //보유중이지 않은 스킬 선택지에 추가
            {
                rand = unPossessionIndexList[Random.Range(0, unPossessionIndexList.Count)]; //35~39줄과 동일한 처리
                selectedChoiceList.Add(unPossessionGimmickList[rand]);
                unPossessionIndexList.Remove(rand);
            }
            else
            {
                rand = GetRandomIndexByLevel(inPossessionIndexList); //보유중인 스킬 중 선택지에 등장하지 않은 스킬의 인덱스 반환
                selectedChoiceList.Add(inPossessionList[inPossessionIndexList[rand]]); //선택지에 추가
                inPossessionIndexList.Remove(inPossessionIndexList[rand]); //보유중인 스킬 인덱스 리스트에서 방금 반환받은 인덱스 제거 (중복 방지를 위한 처리) 
            }
        }
    }
    private int GetRandomIndexByLevel(List<int> refList) //보유 스킬 리스트의 랜덤 인덱스 반환
    {  
        List<int> levelList = new List<int>();
        int refIndex = 0;
        while(refIndex < refList.Count) //선택지에 추가되지 않은 보유 스킬들의 레벨을 중복되지 않게 가져옴
        {
            if (levelList.Contains(inPossessionList[refList[refIndex]].Level)) refIndex++;
            else
            {
                levelList.Add(inPossessionList[refList[refIndex]].Level);
                refIndex++;
            }
        }
        //보유 스킬들의 레벨들을 오름차순으로 정렬함 (가중치 계산을 위해)
        levelList = levelList.OrderBy(o => o).ToList();
        int totalLevel = levelList.Sum(); //가중치 총합을 구함

        int index = -1; //최종적으로 반환할 값

        if (levelList.Count == 1) //보유 스킬들의 레벨이 전부 같은 경우
        {
            index = GetRandomIndexInSameLevelGroup(refList, totalLevel); //랜덤 인덱스 반환
            return index;
        }

        int rand = Random.Range(0, totalLevel); //보유 스킬들의 레벨이 다른 경우 난수 생성
        int total = 0; //가중치를 더해나갈 값 (레벨별 가중치 = 레벨)

        for (int i = 0; i < levelList.Count; i++)
        {
            total += levelList[i]; // 레벨별 가중치를 더해줌

            if (rand < total) //난수가 현재 가중치 값에 속하는 경우 
            {
                index = GetRandomIndexInSameLevelGroup(refList, levelList[i]); //랜덤 인덱스 반환
                break;
            }
        }
        return index;
    }
    private int GetRandomIndexInSameLevelGroup(List<int> refList, int level) //선택된 레벨을 가진 인덱스 중 랜덤한 인덱스 반환
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < refList.Count; i++) //보유 스킬들의 인덱스 리스트 순회
        {
            if (inPossessionList[refList[i]].Level == level) //선택된 레벨과 일치하는 인덱스 요소를 추가
            {
                indexList.Add(i);
            }
        }
        return indexList[Random.Range(0, indexList.Count)]; //선택된 인덱스 들 중 랜덤한 값 반환
    }

    #region 안씀
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
        Debug.Log(name + " 획득");
        LevelUp();
    }
}

