using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private SkillChoicePopUp skillChoicePopUp; //강화 선택지 팝업

    [SerializeField] private List<Skill> selectedChoiceList = new List<Skill>(); //선택지 리스트

    [SerializeField] private List<Skill> inPossessionSkillList = new List<Skill>(); //보유중인 스킬 리스트

    [SerializeField] private List<Skill> unPossessionSkillList = new List<Skill>(); //보유중이지 않은 스킬 리스트

    [SerializeField] private List<Skill> evolutionSkillList = new List<Skill>(); //진화 스킬 리스트
    [SerializeField] private bool[] bEvolutionArray; //진화 스킬이 선택지에 등장할 수 있는지 체크할 배열 (진화 스킬 리스트와 같은 길이, 해당 배열의 0번이 true면 진화 스킬 리스트의 0번 진화 스킬 선택지에 등장 가능)

    [SerializeField] private List<Skill> allSkillList = new List<Skill>();

    [SerializeField] private List<SActive> activeSkillList = new List<SActive>();
    [SerializeField] private List<SPassive> passiveSkillList = new List<SPassive>();
    public List<Skill> SelectedChoiceList { get => selectedChoiceList; }
    public List<SActive> ActiveSkillList { get => activeSkillList; set => activeSkillList = value; }
    public List<Skill> InPossessionSkillList { get => inPossessionSkillList; set => inPossessionSkillList = value; }

    private void Start()
    {
        inPossessionSkillList.Add(FindObjectOfType<AWeapon>());
        allSkillList.Add(inPossessionSkillList[0]);
        activeSkillList.Add(inPossessionSkillList[0].GetComponent<SActive>());
        InitGimmickList();
        //StartCoroutine(Co_SuffleGimmick());
    }
    private void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.M))
        {
            MixSkillOptions();
        }
    }
    public void SetCanEvolution(int index)
    {
        if (evolutionSkillList[index].gameObject.activeSelf) return;
        bEvolutionArray[index] = true;
    }
    public Skill GetActiveSkill(int id)
    {
        foreach (var item in allSkillList)
        {
            if (item.Id == id) return item;
        }
        return null;
    }
    public int GetSkillLevel(int id)
    {
        foreach (var item in allSkillList)
        {
            if (item.Id == id) return item.Level;
        }
        return 0;
    }
    public void Test_SkillLevelUp(int index)
    {
        if(allSkillList[index].Level == 0)
        {
            allSkillList[index].gameObject.SetActive(true);
            allSkillList[index].InitSkill();
            unPossessionSkillList.Remove(allSkillList[index]);
            inPossessionSkillList.Add(allSkillList[index]);
        }
        else
        {
            allSkillList[index].Reinforce();
        }
        allSkillList[index].SetEvlotionCondition(); //진화 가능한지 검사
    }
    private void InitGimmickList()
    {
        foreach (Skill skill in transform.GetComponentsInChildren<Skill>())
        {
            allSkillList.Add(skill);
            switch (skill.ESkillType)
            {
                case ESkillType.Active:
                    activeSkillList.Add(skill.GetComponent<SActive>());
                    unPossessionSkillList.Add(skill);
                    break;
                case ESkillType.Passive:
                    passiveSkillList.Add(skill.GetComponent<SPassive>());
                    unPossessionSkillList.Add(skill);
                    break;
                case ESkillType.Evolution:
                    activeSkillList.Add(skill.GetComponent<SActive>());
                    evolutionSkillList.Add(skill);
                    break;
                default:
                    break;
            }
            skill.gameObject.SetActive(false);
        }
        bEvolutionArray = new bool[evolutionSkillList.Count];
    }
    public void SelectSkill(int index)
    {
        if (selectedChoiceList.Count == 0) return; //null 참조 예외 방지

        // * 선택한 스킬이 진화스킬인 경우의 처리
        if(selectedChoiceList[index].ESkillType == ESkillType.Evolution)
        {
            //진화스킬의 재료 스킬들을 Id 값으로 찾은 후 보유 스킬 리스트에서 제거 & 게임오브젝트 비활성화
            foreach (var item in selectedChoiceList[index].GetComponent<EvolutionChildID>().GetChildID())
            {
                Skill skill = inPossessionSkillList.Find(skill => skill.Id == item);

                inPossessionSkillList.Remove(skill);
                skill.gameObject.SetActive(false);
            }
            //해당 진화스킬의 진화 가능 여부를 false로 해서 선택지에 등장하지 않게 처리
            bEvolutionArray[evolutionSkillList.IndexOf(selectedChoiceList[index])] = false;

            //보유 스킬 리스트에 해당 진화스킬 추가 및 초기화
            inPossessionSkillList.Add(selectedChoiceList[index]);
            selectedChoiceList[index].InitSkill();
        }
        else // * 선택한 스킬이 진화스킬이 아닌 경우의 처리
        {
            //이미 보유중인 스킬인 경우
            if (inPossessionSkillList.Contains(selectedChoiceList[index]))
            {
                selectedChoiceList[index].Reinforce(); //스킬 강화
            }
            else //미보유 스킬인 경우
            {
                inPossessionSkillList.Add(selectedChoiceList[index]); //보유 스킬 리스트에 추가
                unPossessionSkillList.Remove(selectedChoiceList[index]); //미보유 스킬 리스트에서 제거

                selectedChoiceList[index].InitSkill(); //스킬 초기화
            }
            selectedChoiceList[index].SetEvlotionCondition(); //진화 가능한지 검사
        }

        StartCoroutine(skillChoicePopUp.InActivePopUp());
    }
    private void MixSkillOptions()
    {
        selectedChoiceList.Clear(); //선택지 초기화

        List<int> unPossessionIndexList = new List<int>(); //보유중이지 않은 스킬 리스트의 인덱스를 가져옴
        for (int i = 0; i < unPossessionSkillList.Count; i++)
        {
            unPossessionIndexList.Add(i);
        }

        // * 보유중인 스킬이 없는 경우의 처리 (보유중이 아닌 스킬을 동일한 확률로 랜덤하게 3개 중복되지 않게 선택)
        int rand = -1;
        if (inPossessionSkillList.Count == 0) //보유중인 스킬이 없는 경우
        {
            for (int i = 0; i < ConstDefine.SKILL_SELECT_COUNT; i++) //선택지 개수만큼 반복
            {
                //보유중이지 않은 스킬 중 하나의 인덱스를 선택
                rand = unPossessionIndexList[Random.Range(0, unPossessionIndexList.Count)];
                //보유중이지 않은 스킬 리스트에서 선택된 인덱스의 스킬을 선택지에 추가
                selectedChoiceList.Add(unPossessionSkillList[rand]);
                //보유중이지 않은 스킬 인덱스 리스트에서 방금 선택된 인덱스 제거 (중복 방지를 위한 처리)
                unPossessionIndexList.Remove(rand);
            }
            skillChoicePopUp.SetSkillChoicePopUp();
            return;
        }

        // * 진화 가능한 스킬이 있는 경우의 처리 (진화 스킬을 무조건적으로 선택지에 먼저 등장하게 함)
        int currentSelectCount = ConstDefine.SKILL_SELECT_COUNT; //선택지 개수를 가져옴

        List<int> evolutionIndexList = new List<int>(); //선택지에 등장 가능한 진화 스킬의 인덱스를 가져옴
        for (int i = 0; i < evolutionSkillList.Count; i++)
        {
            if (bEvolutionArray[i]) evolutionIndexList.Add(i);
        }
        if (evolutionIndexList.Count >= currentSelectCount) //진화 가능한 스킬의 개수가 선택지 개수보다 많거나 같은 경우
        {
            for (int i = 0; i < currentSelectCount; i++) //선택지 개수만큼 랜덤한 진화 스킬을 선택지에 추가
            {
                rand = evolutionIndexList[Random.Range(0, evolutionIndexList.Count)];
                selectedChoiceList.Add(evolutionSkillList[rand]);
                evolutionIndexList.Remove(rand);
            }
            skillChoicePopUp.SetSkillChoicePopUp();
            return; //함수 종료
        }
        currentSelectCount -= evolutionIndexList.Count; //진화 가능한 스킬의 개수만큼을 선택지 개수에서 빼줌
        for (int i = 0; i < evolutionIndexList.Count; i++) //진화 가능한 스킬의 개수만큼 반복
        {
            selectedChoiceList.Add(evolutionSkillList[evolutionIndexList[i]]); //진화 가능한 스킬들을 선택지에 우선적으로 추가시킴
        }
        

        // * 보유중인 스킬이 있는 경우의 처리 (60 : 40 확률로 보유중인 스킬과 보유중이지 않은 스킬 중 하나를 선택지에 추가시킴)
        List<int> inPossessionIndexList = new List<int>(); //보유중인 스킬 리스트에서 만렙이 아닌 스킬들의 인덱스를 가져옴
        for (int i = 0; i < inPossessionSkillList.Count; i++)
        {
            //만렙이 아니고 진화 스킬이 아닌 스킬 걸러내기          
            if (inPossessionSkillList[i].ESkillType != ESkillType.Evolution && inPossessionSkillList[i].Level != ConstDefine.SKILL_MAX_LEVEL) //만렙이 아니고 진화 스킬이 아닌 스킬 걸러내기
            {
                inPossessionIndexList.Add(i);
            }
        }
        if(IsMaxHaveCount(true)) //보유중인 액티브 스킬이 최대치인 경우
        {
            for (int i = 0; i < unPossessionSkillList.Count; i++)
            {
                //보유중이지 않은 스킬이 등장할 때 액티브 스킬은 등장하지 않게 처리
                if (unPossessionSkillList[i].GetComponent<SActive>())
                {
                    unPossessionIndexList.Remove(i);
                }
            }
        }
        if(IsMaxHaveCount(false)) //보유중인 패시브 스킬이 최대치인 경우
        {
            for (int i = 0; i < unPossessionSkillList.Count; i++)
            {
                //보유중이지 않은 스킬이 등장할 때 패시브 스킬은 등장하지 않게 처리
                if (unPossessionSkillList[i].GetComponent<SPassive>())
                {
                    unPossessionIndexList.Remove(i);
                }
            }
        }
        for (int i = 0; i < currentSelectCount; i++) //선택지 개수 - 진화 스킬 개수 만큼 반복 (진화 가능한 스킬이 없는 경우 3번 반복)
        {
            int weight = Random.Range(1, 101); // 1~100까지 구함

            if (inPossessionIndexList.Count == 0) weight = 0; //보유중인 스킬이 모두 선택지에 추가된 경우의 처리 (보유중이지 않은 스킬만 선택지에 추가되도록)

            if (inPossessionSkillList.Count == ConstDefine.SKILL_MAX_HAVE_COUNT * 2) weight = 100; //보유 스킬이 꽉 찬 경우의 처리 (보유중인 스킬만 선택지에 추가되도록)

            if (weight < 41) //보유중이지 않은 스킬 선택지에 추가
            {
                rand = unPossessionIndexList[Random.Range(0, unPossessionIndexList.Count)]; //35~39줄과 동일한 처리
                selectedChoiceList.Add(unPossessionSkillList[rand]);
                unPossessionIndexList.Remove(rand);
            }
            else
            {
                rand = GetRandomIndexByLevel(inPossessionIndexList); //보유중인 스킬 중 선택지에 등장하지 않은 스킬의 인덱스 반환
                selectedChoiceList.Add(inPossessionSkillList[inPossessionIndexList[rand]]); //선택지에 추가
                inPossessionIndexList.Remove(inPossessionIndexList[rand]); //보유중인 스킬 인덱스 리스트에서 방금 반환받은 인덱스 제거 (중복 방지를 위한 처리) 
            }
        }

        skillChoicePopUp.SetSkillChoicePopUp();
    }
    private int GetRandomIndexByLevel(List<int> refList) //보유 스킬 리스트의 랜덤 인덱스 반환
    {
        List<int> levelList = new List<int>();
        int refIndex = 0;

        while (refIndex < refList.Count) //선택지에 추가되지 않은 보유 스킬들의 레벨을 중복되지 않게 가져옴
        {
            if (levelList.Contains(inPossessionSkillList[refList[refIndex]].Level)) refIndex++;
            else
            {
                levelList.Add(inPossessionSkillList[refList[refIndex]].Level);
                refIndex++;
            }
        }
        //보유 스킬들의 레벨들을 오름차순으로 정렬함 (가중치 계산을 위해)
        levelList = levelList.OrderBy(o => o).ToList();
        int totalLevel = levelList.Sum(); //가중치 총합을 구함

        int index = -1; //최종적으로 반환할 값

        if (levelList.Count == 1) //보유 스킬들의 레벨이 전부 같은 경우
        {
            index = Random.Range(0, refList.Count); //랜덤 인덱스 반환
            return index;
        }

        int rand = Random.Range(0, totalLevel); //보유 스킬들의 레벨이 다른 경우 난수 생성
        int total = 0; //가중치를 더해나갈 값 (레벨별 가중치 = 레벨)

        for (int i = 0; i < levelList.Count; i++)
        {
            total += levelList[i]; // 레벨별 가중치를 더해줌

            if (rand < total) //난수가 현재 가중치 값에 속하는 경우 
            {
                index = GetRandomIndexInSameLevelGroup(refList, levelList[i]); //동일한 레벨들 중에서 랜덤 인덱스 반환
                break;
            }
        }
        return index;
    }
    private int GetRandomIndexInSameLevelGroup(List<int> refList, int level) //선택된 레벨을 가진 인덱스 중 랜덤한 인덱스 반환
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < refList.Count; i++) //보유 스킬들 중 선택지에 등장 가능한 인덱스들을 검사
        {
            if (inPossessionSkillList[refList[i]].Level == level) //선택된 레벨과 일치하는 인덱스 요소를 추가
            {
                indexList.Add(i);
            }
        }
        return indexList[Random.Range(0, indexList.Count)]; //선택된 인덱스 들 중 랜덤한 값 반환
    }
    private bool IsMaxHaveCount(bool isActive) //보유 중인 액티브 스킬과 패시브 스킬의 개수가 최대치인지 여부를 반환
    {
        if(isActive)
        {
            return inPossessionSkillList.Where(w => w.GetComponent<SActive>()).Count() == ConstDefine.SKILL_MAX_HAVE_COUNT;
        }
        return inPossessionSkillList.Where(w => w.GetComponent<SPassive>()).Count() == ConstDefine.SKILL_MAX_HAVE_COUNT;
    }
    //private List<int> GetValidIndexInPossessionList() //보유한 스킬 중에서 선택지에 등장 가능한 스킬의 인덱스만 가져옴
    //{
    //    List<int> list = new List<int>();
    //    if (inPossessionSkillList.Count == 12) return list; //보유 스킬이 꽉 찬 경우의 처리

    //    if(inPossessionSkillList.Count < 6) //액티브, 패시브 모두 꽉 차지 않은 최소 경우의 처리
    //    {
    //        for (int i = 0; i < inPossessionSkillList.Count; i++) 
    //        {
    //            //보유 스킬 리스트에서 만렙이 아닌 스킬의 인덱스만 추가해줌
    //            if (inPossessionSkillList[i].Level < ConstDefine.SKILL_MAX_LEVEL)
    //            {
    //                list.Add(i);
    //            }
    //        }
    //        return list;
    //    }

    //    List<int> activeList = new List<int>();
    //    List<int> passiveList = new List<int>();
    //    for (int i = 0; i < inPossessionSkillList.Count; i++)
    //    {
    //        //보유 스킬 리스트에서 액티브 스킬의 개수 검사
    //        if (inPossessionSkillList[i].GetComponent<SActive>()) activeList.Add(i);
    //        else if (inPossessionSkillList[i].GetComponent<SPassive>()) passiveList.Add(i);
    //    }
    //   // if(activeList.Count < 6)

    //    return list;
    //}
    //private void MixSkillOptions()
    //{
    //    selectedChoiceList.Clear();
    //    List<int> unPossessionIndexList = new List<int>(); //중복 방지를 위한 처리
    //    for (int i = 0; i < unPossessionGimmickList.Count; i++)
    //    {
    //        unPossessionIndexList.Add(i);
    //    }

    //    int rand = -1;
    //    if (inPossessionList.Count == 0)
    //    {      
    //        for (int i = 0; i < 3; i++)
    //        {
    //            rand = unPossessionIndexList[Random.Range(0, unPossessionIndexList.Count)];
    //            selectedChoiceList.Add(unPossessionGimmickList[rand]);
    //            unPossessionIndexList.Remove(rand);
    //        }          
    //        return;
    //    }

    //    List<int> inPossessionIndexList = new List<int>(); //중복 방지를 위한 처리
    //    for (int i = 0; i < inPossessionList.Count; i++)
    //    {
    //        inPossessionIndexList.Add(i);
    //    }

    //    for (int i = 0; i < 3; i++)
    //    {
    //        int weight = Random.Range(1, 101); // 1~100까지 구함

    //        if(weight < 41)
    //        {
    //            rand = unPossessionIndexList[Random.Range(0, unPossessionIndexList.Count)];
    //            selectedChoiceList.Add(unPossessionGimmickList[rand]);
    //            unPossessionIndexList.Remove(rand);
    //        }
    //        else
    //        {
    //            rand = GetRandomIndexInPossessionSkillList();
    //            selectedChoiceList.Add(inPossessionList[rand]);
    //            inPossessionIndexList.Remove(rand);
    //        }
    //    }
    //}
    //private int GetRandomIndexInPossessionSkillList()
    //{
    //    List<int> uniqueLevelList = inPossessionList.Select(s => s.Level).Distinct().ToList();
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
    //private IEnumerator Co_SuffleGimmick() //기믹 선택지 셔플 함수
    //{
    //    yield return new WaitUntil(() => !gimmickReinforcePopUp.gameObject.activeSelf);
    //    Time.timeScale = 1;
    //    selectedChoiceList.Clear(); //선택된 기믹 리스트 클리어
    //    for (int i = 0; i < 3; i++) //등장하는 선택지 개수만큼 반복
    //    {
    //        int rand = UnityEngine.Random.Range(0, 100); // 60 : 40 확률로 보유중인 기믹 : 보유중이 아닌 기믹으로 i번째 선택지가 결정됨

    //        if (inPossessionList.Count == 0) rand = 0; //보유중인 기믹이 없는 경우의 처리

    //        Debug.Log($"첫 분기 확률 검증 : {rand + 1}%");

    //        if (rand > 39) //60% 확률로 보유중인 기믹 선택지 분기로 들어옴
    //        {
    //            Debug.Log($"60% 확률로 보유중인 기믹 {i}번 선택지에 등장");
    //            Skill selectedGimmick = GetRandomGimmickAtInPossessionGimmickList(); //보유중인 기믹 리스트에서 확률에 맞게 랜덤한 기믹을 가져옴

    //            while (selectedChoiceList.Contains(selectedGimmick)) //이미 선택지에 등장한 기믹인 경우 재선택을 하기 위한 루프
    //            {
    //                if (inPossessionList.All(a => selectedChoiceList.Contains(a))) //선택지에 이미 보유중인 모든 기믹이 등장한 경우 보유중이지 않은 기믹에서 선택
    //                {
    //                    Debug.Log("보유중인 기믹이 2개 이하이고 이미 선택지에 전부 등장했기 때문에 보유중이지 않은 기믹 선택 분기로 넘어감");
    //                    rand = 0; //rand를 0으로 처리하여 기믹 중복 추가 방지 및 보유중이지 않은 기믹 선택 분기 접근
    //                    break;
    //                }
    //                Debug.Log("보유 기믹 중 이미 선택지에 등장한 기믹이라 기믹 랜덤 재선택");
    //                selectedGimmick = GetRandomGimmickAtInPossessionGimmickList(); //다시 랜덤한 기믹을 가져옴 (중복되지 않을 때까지)
    //                yield return null;
    //            }
    //            if (rand > 0)
    //            {
    //                selectedChoiceList.Add(selectedGimmick); //중복 기믹이 아닌 경우에만 선택지에 추가
    //            }
    //        }
    //        if (rand < 40) //40% 확률로 보유중이 아닌 기믹 선택지 분기로 들어옴
    //        {
    //            Debug.Log($"40% 확률로 보유중이지 않은 기믹 {i}번 선택지에 등장");
    //            Skill selectedGimmick = GetRandomGimmickAtUnPossessionGimmickList(); //보유중이 아닌 기믹 리스트에서 확률에 맞게 랜덤한 기믹을 가져옴
    //            while (selectedChoiceList.Contains(selectedGimmick)) //마찬가지로 선택지 중복 등장을 방지하기 위한 루프
    //            {
    //                Debug.Log("보유하지 않은 기믹 중 이미 선택지에 등장한 기믹이라 기믹 랜덤 재선택");
    //                selectedGimmick = GetRandomGimmickAtUnPossessionGimmickList();
    //                yield return null;
    //            }
    //            selectedChoiceList.Add(selectedGimmick); //이후 중복 방지 처리가 된 기믹을 선택지에 추가
    //        }
    //        yield return null;
    //    }
    //}
    //private Skill GetRandomGimmickAtInPossessionGimmickList() //보유중인 기믹들을 레벨별 확률에 맞게 랜덤하게 선택하여 반환하는 함수
    //{
    //    List<int> uniqueLevelList = inPossessionList.Select(s => s.Level).Distinct().ToList();
    //    int totalLevel = uniqueLevelList.Sum();

    //    int index = -1; //보유중인 기믹의 인덱스 중 확률과 조건에 맞게 배출될 랜덤 인덱스

    //    if (uniqueLevelList.Count == 1)
    //    {
    //        Debug.Log("보유중인 스킬의 레벨이 전부 동일한 레벨이기 때문에 해당 레벨에서 랜덤한 스킬 선택");
    //        index = GetRandomIndexInPossessionGimickListByLevel(totalLevel);
    //    }
    //    else
    //    {
    //        Debug.Log("보유중인 스킬의 레벨이 서로 다르기 때문에 가중치에 따른 랜덤한 스킬 선택\n가중치 총량 : " + totalLevel);
    //        int rand = UnityEngine.Random.Range(0, totalLevel);
    //        Debug.Log("등장 가중치 : " + rand);
    //        int total = 0;
    //        for (int i = 0; i < uniqueLevelList.Count; i++)
    //        {
    //            total += uniqueLevelList[i];
    //            Debug.Log("현재 총량 : " + total);
    //            if (rand <= total)
    //            {
    //                Debug.Log("현재 총량이 가중치보다 크거나 같기 때문에 현재 총량을 가중치로 가지는 레벨 스킬 선택");
    //                Debug.Log("현재 선택된 스킬 레벨 : " + uniqueLevelList[i]);
    //                index = GetRandomIndexInPossessionGimickListByLevel(uniqueLevelList[i]);
    //                break;
    //            }
    //        }
    //    }
    //    Debug.Log($"선택된 인덱스 : {index} / 선택된 스킬 : {inPossessionList[index]}");
    //    return inPossessionList[index]; //확률과 조건에 맞게 선택된 기믹 반환
    //}
    //private int GetRandomIndexInPossessionGimickListByLevel(int level) //보유중인 기믹들 중 레벨에 맞게 시작, 끝 인덱스 추출 및 사이 난수값 생성하여 반환 
    //{
    //    int min = inPossessionList.FindIndex(f => f.Level == level); //쿼리문으로 level이 조건에 맞는 인덱스를 반환
    //    int max = inPossessionList.FindLastIndex(f => f.Level == level);

    //    Debug.Log("선택된 스킬 레벨 총 갯수 : " + (max - min + 1));
    //    int rand = UnityEngine.Random.Range(min, max + 1);
    //    Debug.Log("선택된 스킬 레벨 중 선택 된 스킬 : " + rand + "번 째 스킬");
    //    return rand; //인덱스 사이 난수값을 반환
    //}

    //private Skill GetRandomGimmickAtUnPossessionGimmickList() //보유중이 아닌 기믹들 중 랜덤한 기믹을 선택하여 반환하는 함수
    //{
    //    float rand = UnityEngine.Random.Range(0f, 100f); //랜덤 기믹 선택을 위해 생성한 난수
    //    float share = (float)100 / (float)unPossessionGimmickList.Count; //각 기믹들의 균일한 가중치값. 모든 기믹들의 가중치는 균일하게 (100 / 보유중이 아닌 기믹 수) 로 분배됨. float로 정확한 계산

    //    int index = -1; //선택된 기믹을 가리킬 인덱스

    //    float total = 0; //계속해서 가중치를 더해나갈 총 값.

    //    Debug.Log($"보유하지 않은 기믹 선택 분기 확률 검증\n등장한 확률 : {rand}\n각 기믹별 확률 {share}");

    //    while (true) //더해나간 가중치 총 값이 난수보다 크거나 같아지면 루프 탈출
    //    {
    //        if (total >= rand) break;

    //        total += share; //가중치를 계속해서 더해나감

    //        Debug.Log($"현재 총 값 : {total}");

    //        index++; //가중치를 더해나갈 때마다 인덱스 1씩 증가
    //    }
    //    Debug.Log($"선택된 기믹 :  {unPossessionGimmickList[index].name}");
    //    return unPossessionGimmickList[index].GetComponent<Skill>(); //선택된 기믹 반환
    //}
}
