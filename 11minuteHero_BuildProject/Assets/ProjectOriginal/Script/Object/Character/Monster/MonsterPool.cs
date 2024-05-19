using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour //몬스터를 풀링, 관리하는 클래스. 몬스터 풀의 참조는 몬스터 스포너에서만 필요하기 때문에 굳이 싱글턴으로 둘 필요가 없다.
{
    [SerializeField] private GameObject[] monsterPrefabArray; //몬스터 프리팹 배열
    private Transform field; //생성된 몬스터의 transform parent가 될 참조

    private Coroutine[] spawnCoroutineArray;
    private List<ObjectPool<NormalMonster>> monsterPoolList = new List<ObjectPool<NormalMonster>>();
    private List<NormalMonster> activatedMonsterList = new List<NormalMonster>();

    private float[] createCycle;
    private float[] createCycleDecrease;
    private float[] createStartTime;
    private float[] createEndTime;

    private int[] createCount;
    private int[] createCountIncrease;
    private bool[] isActive;

    [Range(25, 50)]
    [SerializeField] private float monsterSpawnDistance;

    private WaitForSeconds[] monsterSpawnIntervalArray;
    public List<NormalMonster> ActivatedMonsterList { get => activatedMonsterList; }

    public int summonCountForTest; //풀에 생성할 몬스터 수
    private void Awake()
    { 
        field = GameObject.Find("Field").transform;
    }
    private void Start()
    {
        CreateNewMonster();
        spawnCoroutineArray = new Coroutine[monsterPrefabArray.Length];
        createCycle = new float[monsterPrefabArray.Length];
        createCycleDecrease = new float[monsterPrefabArray.Length];
        createStartTime = new float[monsterPrefabArray.Length];
        createEndTime = new float[monsterPrefabArray.Length];
        createCount = new int[monsterPrefabArray.Length];
        createCountIncrease = new int[monsterPrefabArray.Length];
        monsterSpawnIntervalArray = new WaitForSeconds[monsterPrefabArray.Length];
        isActive = new bool[monsterPrefabArray.Length];

        ReadCSVData();

        for (int i = 0; i < spawnCoroutineArray.Length; i++)
        {
            spawnCoroutineArray[i] = StartCoroutine(Co_SpawnMonster(i));
        }
        StartCoroutine(Co_UpdateData());
    }
    private void ReadCSVData()
    {
        for (int i = 0; i < monsterPrefabArray.Length; i++)
        {
            createCycle[i] = InGameManager.Instance.CSVManager.GetCSVData<float>(5, i + 1, 5);
            Debug.Log(createCycle[i]);
            monsterSpawnIntervalArray[i] = new WaitForSeconds(createCycle[i]);
            createCycleDecrease[i] = InGameManager.Instance.CSVManager.GetCSVData<float>(5, i + 1, 6);

            createCount[i] = InGameManager.Instance.CSVManager.GetCSVData<int>(5, i + 1, 7);
            createCountIncrease[i] = InGameManager.Instance.CSVManager.GetCSVData<int>(5, i + 1, 8);


            createStartTime[i] = InGameManager.Instance.CSVManager.GetCSVData<float>(5, i + 1, 9);
            createEndTime[i] = InGameManager.Instance.CSVManager.GetCSVData<float>(5, i + 1, 10);
        }
    }
    private void CreateNewMonster() //count만큼 몬스터 생성
    {
        for (int i = 0; i < monsterPrefabArray.Length; i++)
        {
            monsterPoolList.Add(new ObjectPool<NormalMonster>(monsterPrefabArray[i], summonCountForTest, transform));
            monsterPoolList[i].CreateObject();
            foreach (var item in monsterPoolList[i].Pool)
            {
                item.InitMonsterData();
                item.InitDamageUIContainer();
                item.ReturnIndex = i;
            }
        }
    }
    private NormalMonster GetMonster(Vector3 pos) //풀에서 몬스터를 꺼내오는 함수
    {
        int rand = Random.Range(0, monsterPrefabArray.Length);

        monsterPoolList[rand].IsValid();

        NormalMonster monster = monsterPoolList[rand].GetObject(); //풀에서 디큐

        activatedMonsterList.Add(monster); //활성화되어있는 몬스터 리스트에 넣어줌

        monster.transform.SetParent(field); //활성화 관련 초기화
        monster.transform.position = pos; //몬스터 위치 설정
        monster.gameObject.SetActive(true);

        monster.ResetMonster(); //몬스터를 유효한 상태로 재설정

        InGameManager.Instance.MonsterList.Add(monster);

        return monster;
    }
    public NormalMonster GetMonster(Vector3 pos, int index) //풀에서 몬스터를 꺼내오는 함수
    {
        if(!monsterPoolList[index].IsValid())
        {
            foreach (var item in monsterPoolList[index].Pool)
            {
                item.InitMonsterData();
                item.InitDamageUIContainer();
                item.ReturnIndex = index;
            }
        }

        NormalMonster monster = monsterPoolList[index].GetObject(); //풀에서 디큐

        activatedMonsterList.Add(monster); //활성화되어있는 몬스터 리스트에 넣어줌

        monster.transform.SetParent(field); //활성화 관련 초기화
        monster.transform.position = pos; //몬스터 위치 설정
        monster.gameObject.SetActive(true);

        monster.ResetMonster(); //몬스터를 유효한 상태로 재설정

        InGameManager.Instance.MonsterList.Add(monster);

        return monster;
    }
    public void ReturnMonster(NormalMonster monster, int index) //몬스터를 풀로 되돌리는 함수
    {
        activatedMonsterList.Remove(monster); //활성화되어있는 몬스터 리스트에서 몬스터 삭제

        monster.gameObject.SetActive(false); //풀로 되돌리기 관련 초기화
        monster.transform.SetParent(transform);

        monsterPoolList[index].ReturnObject(monster);
    }
    private IEnumerator Co_SpawnMonster(int index)
    {
        yield return new WaitForSeconds(createStartTime[index]);
        isActive[index] = true;
        Vector3 rotDir;
        while(InGameManager.Instance.Timer < createEndTime[index])
        {
            if(!InGameManager.Instance.bTimeStop)
            {
                for (int i = 0; i < createCount[index]; i++)
                {
                    rotDir = Quaternion.Euler(0, Random.Range(0, 361), 0) * Vector3.forward; //랜덤한 방향 설정
                    GetMonster(InGameManager.Instance.Player.transform.position + rotDir.normalized * monsterSpawnDistance, index);
                }
            }
            yield return monsterSpawnIntervalArray[index];
        }
    }
    private IEnumerator Co_UpdateData()
    {
        WaitForSeconds wait = new WaitForSeconds(60);
        while(!InGameManager.Instance.bAppearBoss)
        {
            yield return wait;
            for (int i = 0; i < monsterPrefabArray.Length; i++)
            {
                if(isActive[i])
                {
                    createCycle[i] -= createCycleDecrease[i];
                    monsterSpawnIntervalArray[i] = new WaitForSeconds(createCycle[i]);
                    createCount[i] += createCountIncrease[i];
                }
            }
        }
    }

}
