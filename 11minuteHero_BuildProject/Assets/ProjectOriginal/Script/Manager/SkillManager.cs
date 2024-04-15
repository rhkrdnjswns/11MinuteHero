using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private SkillChoicePopUp skillChoicePopUp; //��ȭ ������ �˾�

    [SerializeField] private List<Skill> selectedChoiceList = new List<Skill>(); //������ ����Ʈ

    [SerializeField] private List<Skill> inPossessionSkillList = new List<Skill>(); //�������� ��ų ����Ʈ

    [SerializeField] private List<Skill> unPossessionSkillList = new List<Skill>(); //���������� ���� ��ų ����Ʈ

    [SerializeField] private List<Skill> evolutionSkillList = new List<Skill>(); //��ȭ ��ų ����Ʈ
    [SerializeField] private bool[] bEvolutionArray; //��ȭ ��ų�� �������� ������ �� �ִ��� üũ�� �迭 (��ȭ ��ų ����Ʈ�� ���� ����, �ش� �迭�� 0���� true�� ��ȭ ��ų ����Ʈ�� 0�� ��ȭ ��ų �������� ���� ����)

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
        allSkillList[index].SetEvlotionCondition(); //��ȭ �������� �˻�
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
        if (selectedChoiceList.Count == 0) return; //null ���� ���� ����

        // * ������ ��ų�� ��ȭ��ų�� ����� ó��
        if(selectedChoiceList[index].ESkillType == ESkillType.Evolution)
        {
            //��ȭ��ų�� ��� ��ų���� Id ������ ã�� �� ���� ��ų ����Ʈ���� ���� & ���ӿ�����Ʈ ��Ȱ��ȭ
            foreach (var item in selectedChoiceList[index].GetComponent<EvolutionChildID>().GetChildID())
            {
                Skill skill = inPossessionSkillList.Find(skill => skill.Id == item);

                inPossessionSkillList.Remove(skill);
                skill.gameObject.SetActive(false);
            }
            //�ش� ��ȭ��ų�� ��ȭ ���� ���θ� false�� �ؼ� �������� �������� �ʰ� ó��
            bEvolutionArray[evolutionSkillList.IndexOf(selectedChoiceList[index])] = false;

            //���� ��ų ����Ʈ�� �ش� ��ȭ��ų �߰� �� �ʱ�ȭ
            inPossessionSkillList.Add(selectedChoiceList[index]);
            selectedChoiceList[index].InitSkill();
        }
        else // * ������ ��ų�� ��ȭ��ų�� �ƴ� ����� ó��
        {
            //�̹� �������� ��ų�� ���
            if (inPossessionSkillList.Contains(selectedChoiceList[index]))
            {
                selectedChoiceList[index].Reinforce(); //��ų ��ȭ
            }
            else //�̺��� ��ų�� ���
            {
                inPossessionSkillList.Add(selectedChoiceList[index]); //���� ��ų ����Ʈ�� �߰�
                unPossessionSkillList.Remove(selectedChoiceList[index]); //�̺��� ��ų ����Ʈ���� ����

                selectedChoiceList[index].InitSkill(); //��ų �ʱ�ȭ
            }
            selectedChoiceList[index].SetEvlotionCondition(); //��ȭ �������� �˻�
        }

        StartCoroutine(skillChoicePopUp.InActivePopUp());
    }
    private void MixSkillOptions()
    {
        selectedChoiceList.Clear(); //������ �ʱ�ȭ

        List<int> unPossessionIndexList = new List<int>(); //���������� ���� ��ų ����Ʈ�� �ε����� ������
        for (int i = 0; i < unPossessionSkillList.Count; i++)
        {
            unPossessionIndexList.Add(i);
        }

        // * �������� ��ų�� ���� ����� ó�� (�������� �ƴ� ��ų�� ������ Ȯ���� �����ϰ� 3�� �ߺ����� �ʰ� ����)
        int rand = -1;
        if (inPossessionSkillList.Count == 0) //�������� ��ų�� ���� ���
        {
            for (int i = 0; i < ConstDefine.SKILL_SELECT_COUNT; i++) //������ ������ŭ �ݺ�
            {
                //���������� ���� ��ų �� �ϳ��� �ε����� ����
                rand = unPossessionIndexList[Random.Range(0, unPossessionIndexList.Count)];
                //���������� ���� ��ų ����Ʈ���� ���õ� �ε����� ��ų�� �������� �߰�
                selectedChoiceList.Add(unPossessionSkillList[rand]);
                //���������� ���� ��ų �ε��� ����Ʈ���� ��� ���õ� �ε��� ���� (�ߺ� ������ ���� ó��)
                unPossessionIndexList.Remove(rand);
            }
            skillChoicePopUp.SetSkillChoicePopUp();
            return;
        }

        // * ��ȭ ������ ��ų�� �ִ� ����� ó�� (��ȭ ��ų�� ������������ �������� ���� �����ϰ� ��)
        int currentSelectCount = ConstDefine.SKILL_SELECT_COUNT; //������ ������ ������

        List<int> evolutionIndexList = new List<int>(); //�������� ���� ������ ��ȭ ��ų�� �ε����� ������
        for (int i = 0; i < evolutionSkillList.Count; i++)
        {
            if (bEvolutionArray[i]) evolutionIndexList.Add(i);
        }
        if (evolutionIndexList.Count >= currentSelectCount) //��ȭ ������ ��ų�� ������ ������ �������� ���ų� ���� ���
        {
            for (int i = 0; i < currentSelectCount; i++) //������ ������ŭ ������ ��ȭ ��ų�� �������� �߰�
            {
                rand = evolutionIndexList[Random.Range(0, evolutionIndexList.Count)];
                selectedChoiceList.Add(evolutionSkillList[rand]);
                evolutionIndexList.Remove(rand);
            }
            skillChoicePopUp.SetSkillChoicePopUp();
            return; //�Լ� ����
        }
        currentSelectCount -= evolutionIndexList.Count; //��ȭ ������ ��ų�� ������ŭ�� ������ �������� ����
        for (int i = 0; i < evolutionIndexList.Count; i++) //��ȭ ������ ��ų�� ������ŭ �ݺ�
        {
            selectedChoiceList.Add(evolutionSkillList[evolutionIndexList[i]]); //��ȭ ������ ��ų���� �������� �켱������ �߰���Ŵ
        }
        

        // * �������� ��ų�� �ִ� ����� ó�� (60 : 40 Ȯ���� �������� ��ų�� ���������� ���� ��ų �� �ϳ��� �������� �߰���Ŵ)
        List<int> inPossessionIndexList = new List<int>(); //�������� ��ų ����Ʈ���� ������ �ƴ� ��ų���� �ε����� ������
        for (int i = 0; i < inPossessionSkillList.Count; i++)
        {
            //������ �ƴϰ� ��ȭ ��ų�� �ƴ� ��ų �ɷ�����          
            if (inPossessionSkillList[i].ESkillType != ESkillType.Evolution && inPossessionSkillList[i].Level != ConstDefine.SKILL_MAX_LEVEL) //������ �ƴϰ� ��ȭ ��ų�� �ƴ� ��ų �ɷ�����
            {
                inPossessionIndexList.Add(i);
            }
        }
        if(IsMaxHaveCount(true)) //�������� ��Ƽ�� ��ų�� �ִ�ġ�� ���
        {
            for (int i = 0; i < unPossessionSkillList.Count; i++)
            {
                //���������� ���� ��ų�� ������ �� ��Ƽ�� ��ų�� �������� �ʰ� ó��
                if (unPossessionSkillList[i].GetComponent<SActive>())
                {
                    unPossessionIndexList.Remove(i);
                }
            }
        }
        if(IsMaxHaveCount(false)) //�������� �нú� ��ų�� �ִ�ġ�� ���
        {
            for (int i = 0; i < unPossessionSkillList.Count; i++)
            {
                //���������� ���� ��ų�� ������ �� �нú� ��ų�� �������� �ʰ� ó��
                if (unPossessionSkillList[i].GetComponent<SPassive>())
                {
                    unPossessionIndexList.Remove(i);
                }
            }
        }
        for (int i = 0; i < currentSelectCount; i++) //������ ���� - ��ȭ ��ų ���� ��ŭ �ݺ� (��ȭ ������ ��ų�� ���� ��� 3�� �ݺ�)
        {
            int weight = Random.Range(1, 101); // 1~100���� ����

            if (inPossessionIndexList.Count == 0) weight = 0; //�������� ��ų�� ��� �������� �߰��� ����� ó�� (���������� ���� ��ų�� �������� �߰��ǵ���)

            if (inPossessionSkillList.Count == ConstDefine.SKILL_MAX_HAVE_COUNT * 2) weight = 100; //���� ��ų�� �� �� ����� ó�� (�������� ��ų�� �������� �߰��ǵ���)

            if (weight < 41) //���������� ���� ��ų �������� �߰�
            {
                rand = unPossessionIndexList[Random.Range(0, unPossessionIndexList.Count)]; //35~39�ٰ� ������ ó��
                selectedChoiceList.Add(unPossessionSkillList[rand]);
                unPossessionIndexList.Remove(rand);
            }
            else
            {
                rand = GetRandomIndexByLevel(inPossessionIndexList); //�������� ��ų �� �������� �������� ���� ��ų�� �ε��� ��ȯ
                selectedChoiceList.Add(inPossessionSkillList[inPossessionIndexList[rand]]); //�������� �߰�
                inPossessionIndexList.Remove(inPossessionIndexList[rand]); //�������� ��ų �ε��� ����Ʈ���� ��� ��ȯ���� �ε��� ���� (�ߺ� ������ ���� ó��) 
            }
        }

        skillChoicePopUp.SetSkillChoicePopUp();
    }
    private int GetRandomIndexByLevel(List<int> refList) //���� ��ų ����Ʈ�� ���� �ε��� ��ȯ
    {
        List<int> levelList = new List<int>();
        int refIndex = 0;

        while (refIndex < refList.Count) //�������� �߰����� ���� ���� ��ų���� ������ �ߺ����� �ʰ� ������
        {
            if (levelList.Contains(inPossessionSkillList[refList[refIndex]].Level)) refIndex++;
            else
            {
                levelList.Add(inPossessionSkillList[refList[refIndex]].Level);
                refIndex++;
            }
        }
        //���� ��ų���� �������� ������������ ������ (����ġ ����� ����)
        levelList = levelList.OrderBy(o => o).ToList();
        int totalLevel = levelList.Sum(); //����ġ ������ ����

        int index = -1; //���������� ��ȯ�� ��

        if (levelList.Count == 1) //���� ��ų���� ������ ���� ���� ���
        {
            index = Random.Range(0, refList.Count); //���� �ε��� ��ȯ
            return index;
        }

        int rand = Random.Range(0, totalLevel); //���� ��ų���� ������ �ٸ� ��� ���� ����
        int total = 0; //����ġ�� ���س��� �� (������ ����ġ = ����)

        for (int i = 0; i < levelList.Count; i++)
        {
            total += levelList[i]; // ������ ����ġ�� ������

            if (rand < total) //������ ���� ����ġ ���� ���ϴ� ��� 
            {
                index = GetRandomIndexInSameLevelGroup(refList, levelList[i]); //������ ������ �߿��� ���� �ε��� ��ȯ
                break;
            }
        }
        return index;
    }
    private int GetRandomIndexInSameLevelGroup(List<int> refList, int level) //���õ� ������ ���� �ε��� �� ������ �ε��� ��ȯ
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < refList.Count; i++) //���� ��ų�� �� �������� ���� ������ �ε������� �˻�
        {
            if (inPossessionSkillList[refList[i]].Level == level) //���õ� ������ ��ġ�ϴ� �ε��� ��Ҹ� �߰�
            {
                indexList.Add(i);
            }
        }
        return indexList[Random.Range(0, indexList.Count)]; //���õ� �ε��� �� �� ������ �� ��ȯ
    }
    private bool IsMaxHaveCount(bool isActive) //���� ���� ��Ƽ�� ��ų�� �нú� ��ų�� ������ �ִ�ġ���� ���θ� ��ȯ
    {
        if(isActive)
        {
            return inPossessionSkillList.Where(w => w.GetComponent<SActive>()).Count() == ConstDefine.SKILL_MAX_HAVE_COUNT;
        }
        return inPossessionSkillList.Where(w => w.GetComponent<SPassive>()).Count() == ConstDefine.SKILL_MAX_HAVE_COUNT;
    }
    //private List<int> GetValidIndexInPossessionList() //������ ��ų �߿��� �������� ���� ������ ��ų�� �ε����� ������
    //{
    //    List<int> list = new List<int>();
    //    if (inPossessionSkillList.Count == 12) return list; //���� ��ų�� �� �� ����� ó��

    //    if(inPossessionSkillList.Count < 6) //��Ƽ��, �нú� ��� �� ���� ���� �ּ� ����� ó��
    //    {
    //        for (int i = 0; i < inPossessionSkillList.Count; i++) 
    //        {
    //            //���� ��ų ����Ʈ���� ������ �ƴ� ��ų�� �ε����� �߰�����
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
    //        //���� ��ų ����Ʈ���� ��Ƽ�� ��ų�� ���� �˻�
    //        if (inPossessionSkillList[i].GetComponent<SActive>()) activeList.Add(i);
    //        else if (inPossessionSkillList[i].GetComponent<SPassive>()) passiveList.Add(i);
    //    }
    //   // if(activeList.Count < 6)

    //    return list;
    //}
    //private void MixSkillOptions()
    //{
    //    selectedChoiceList.Clear();
    //    List<int> unPossessionIndexList = new List<int>(); //�ߺ� ������ ���� ó��
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

    //    List<int> inPossessionIndexList = new List<int>(); //�ߺ� ������ ���� ó��
    //    for (int i = 0; i < inPossessionList.Count; i++)
    //    {
    //        inPossessionIndexList.Add(i);
    //    }

    //    for (int i = 0; i < 3; i++)
    //    {
    //        int weight = Random.Range(1, 101); // 1~100���� ����

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
    //private IEnumerator Co_SuffleGimmick() //��� ������ ���� �Լ�
    //{
    //    yield return new WaitUntil(() => !gimmickReinforcePopUp.gameObject.activeSelf);
    //    Time.timeScale = 1;
    //    selectedChoiceList.Clear(); //���õ� ��� ����Ʈ Ŭ����
    //    for (int i = 0; i < 3; i++) //�����ϴ� ������ ������ŭ �ݺ�
    //    {
    //        int rand = UnityEngine.Random.Range(0, 100); // 60 : 40 Ȯ���� �������� ��� : �������� �ƴ� ������� i��° �������� ������

    //        if (inPossessionList.Count == 0) rand = 0; //�������� ����� ���� ����� ó��

    //        Debug.Log($"ù �б� Ȯ�� ���� : {rand + 1}%");

    //        if (rand > 39) //60% Ȯ���� �������� ��� ������ �б�� ����
    //        {
    //            Debug.Log($"60% Ȯ���� �������� ��� {i}�� �������� ����");
    //            Skill selectedGimmick = GetRandomGimmickAtInPossessionGimmickList(); //�������� ��� ����Ʈ���� Ȯ���� �°� ������ ����� ������

    //            while (selectedChoiceList.Contains(selectedGimmick)) //�̹� �������� ������ ����� ��� �缱���� �ϱ� ���� ����
    //            {
    //                if (inPossessionList.All(a => selectedChoiceList.Contains(a))) //�������� �̹� �������� ��� ����� ������ ��� ���������� ���� ��Ϳ��� ����
    //                {
    //                    Debug.Log("�������� ����� 2�� �����̰� �̹� �������� ���� �����߱� ������ ���������� ���� ��� ���� �б�� �Ѿ");
    //                    rand = 0; //rand�� 0���� ó���Ͽ� ��� �ߺ� �߰� ���� �� ���������� ���� ��� ���� �б� ����
    //                    break;
    //                }
    //                Debug.Log("���� ��� �� �̹� �������� ������ ����̶� ��� ���� �缱��");
    //                selectedGimmick = GetRandomGimmickAtInPossessionGimmickList(); //�ٽ� ������ ����� ������ (�ߺ����� ���� ������)
    //                yield return null;
    //            }
    //            if (rand > 0)
    //            {
    //                selectedChoiceList.Add(selectedGimmick); //�ߺ� ����� �ƴ� ��쿡�� �������� �߰�
    //            }
    //        }
    //        if (rand < 40) //40% Ȯ���� �������� �ƴ� ��� ������ �б�� ����
    //        {
    //            Debug.Log($"40% Ȯ���� ���������� ���� ��� {i}�� �������� ����");
    //            Skill selectedGimmick = GetRandomGimmickAtUnPossessionGimmickList(); //�������� �ƴ� ��� ����Ʈ���� Ȯ���� �°� ������ ����� ������
    //            while (selectedChoiceList.Contains(selectedGimmick)) //���������� ������ �ߺ� ������ �����ϱ� ���� ����
    //            {
    //                Debug.Log("�������� ���� ��� �� �̹� �������� ������ ����̶� ��� ���� �缱��");
    //                selectedGimmick = GetRandomGimmickAtUnPossessionGimmickList();
    //                yield return null;
    //            }
    //            selectedChoiceList.Add(selectedGimmick); //���� �ߺ� ���� ó���� �� ����� �������� �߰�
    //        }
    //        yield return null;
    //    }
    //}
    //private Skill GetRandomGimmickAtInPossessionGimmickList() //�������� ��͵��� ������ Ȯ���� �°� �����ϰ� �����Ͽ� ��ȯ�ϴ� �Լ�
    //{
    //    List<int> uniqueLevelList = inPossessionList.Select(s => s.Level).Distinct().ToList();
    //    int totalLevel = uniqueLevelList.Sum();

    //    int index = -1; //�������� ����� �ε��� �� Ȯ���� ���ǿ� �°� ����� ���� �ε���

    //    if (uniqueLevelList.Count == 1)
    //    {
    //        Debug.Log("�������� ��ų�� ������ ���� ������ �����̱� ������ �ش� �������� ������ ��ų ����");
    //        index = GetRandomIndexInPossessionGimickListByLevel(totalLevel);
    //    }
    //    else
    //    {
    //        Debug.Log("�������� ��ų�� ������ ���� �ٸ��� ������ ����ġ�� ���� ������ ��ų ����\n����ġ �ѷ� : " + totalLevel);
    //        int rand = UnityEngine.Random.Range(0, totalLevel);
    //        Debug.Log("���� ����ġ : " + rand);
    //        int total = 0;
    //        for (int i = 0; i < uniqueLevelList.Count; i++)
    //        {
    //            total += uniqueLevelList[i];
    //            Debug.Log("���� �ѷ� : " + total);
    //            if (rand <= total)
    //            {
    //                Debug.Log("���� �ѷ��� ����ġ���� ũ�ų� ���� ������ ���� �ѷ��� ����ġ�� ������ ���� ��ų ����");
    //                Debug.Log("���� ���õ� ��ų ���� : " + uniqueLevelList[i]);
    //                index = GetRandomIndexInPossessionGimickListByLevel(uniqueLevelList[i]);
    //                break;
    //            }
    //        }
    //    }
    //    Debug.Log($"���õ� �ε��� : {index} / ���õ� ��ų : {inPossessionList[index]}");
    //    return inPossessionList[index]; //Ȯ���� ���ǿ� �°� ���õ� ��� ��ȯ
    //}
    //private int GetRandomIndexInPossessionGimickListByLevel(int level) //�������� ��͵� �� ������ �°� ����, �� �ε��� ���� �� ���� ������ �����Ͽ� ��ȯ 
    //{
    //    int min = inPossessionList.FindIndex(f => f.Level == level); //���������� level�� ���ǿ� �´� �ε����� ��ȯ
    //    int max = inPossessionList.FindLastIndex(f => f.Level == level);

    //    Debug.Log("���õ� ��ų ���� �� ���� : " + (max - min + 1));
    //    int rand = UnityEngine.Random.Range(min, max + 1);
    //    Debug.Log("���õ� ��ų ���� �� ���� �� ��ų : " + rand + "�� ° ��ų");
    //    return rand; //�ε��� ���� �������� ��ȯ
    //}

    //private Skill GetRandomGimmickAtUnPossessionGimmickList() //�������� �ƴ� ��͵� �� ������ ����� �����Ͽ� ��ȯ�ϴ� �Լ�
    //{
    //    float rand = UnityEngine.Random.Range(0f, 100f); //���� ��� ������ ���� ������ ����
    //    float share = (float)100 / (float)unPossessionGimmickList.Count; //�� ��͵��� ������ ����ġ��. ��� ��͵��� ����ġ�� �����ϰ� (100 / �������� �ƴ� ��� ��) �� �й��. float�� ��Ȯ�� ���

    //    int index = -1; //���õ� ����� ����ų �ε���

    //    float total = 0; //����ؼ� ����ġ�� ���س��� �� ��.

    //    Debug.Log($"�������� ���� ��� ���� �б� Ȯ�� ����\n������ Ȯ�� : {rand}\n�� ��ͺ� Ȯ�� {share}");

    //    while (true) //���س��� ����ġ �� ���� �������� ũ�ų� �������� ���� Ż��
    //    {
    //        if (total >= rand) break;

    //        total += share; //����ġ�� ����ؼ� ���س���

    //        Debug.Log($"���� �� �� : {total}");

    //        index++; //����ġ�� ���س��� ������ �ε��� 1�� ����
    //    }
    //    Debug.Log($"���õ� ��� :  {unPossessionGimmickList[index].name}");
    //    return unPossessionGimmickList[index].GetComponent<Skill>(); //���õ� ��� ��ȯ
    //}
}
