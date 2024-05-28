using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterPool : MonoBehaviour //���͸� Ǯ��, �����ϴ� Ŭ����. ���� Ǯ�� ������ ���� �����ʿ����� �ʿ��ϱ� ������ ���� �̱������� �� �ʿ䰡 ����.
{
    [SerializeField] private GameObject[] monsterPrefabArray; //���� ������ �迭
    private Transform field; //������ ������ transform parent�� �� ����

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

    public int[] waveWeightArray;

    public Cartel cartel;

    [Range(25, 50)]
    [SerializeField] private float monsterSpawnDistance;

    private WaitForSeconds[] monsterSpawnIntervalArray;
    public List<NormalMonster> ActivatedMonsterList { get => activatedMonsterList; }

    public int summonCountForTest; //Ǯ�� ������ ���� ��
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
        StartCoroutine(Co_ExecuteRandomWave());
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
    private void CreateNewMonster() //count��ŭ ���� ����
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
    private NormalMonster GetMonster(Vector3 pos) //Ǯ���� ���͸� �������� �Լ�
    {
        int rand = Random.Range(0, monsterPrefabArray.Length);

        monsterPoolList[rand].IsValid();

        NormalMonster monster = monsterPoolList[rand].GetObject(); //Ǯ���� ��ť

        activatedMonsterList.Add(monster); //Ȱ��ȭ�Ǿ��ִ� ���� ����Ʈ�� �־���

        monster.transform.SetParent(field); //Ȱ��ȭ ���� �ʱ�ȭ
        monster.transform.position = pos; //���� ��ġ ����
        monster.gameObject.SetActive(true);

        monster.ResetMonster(); //���͸� ��ȿ�� ���·� �缳��

        InGameManager.Instance.MonsterList.Add(monster);

        return monster;
    }
    public NormalMonster GetMonster(Vector3 pos, int index) //Ǯ���� ���͸� �������� �Լ�
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

        NormalMonster monster = monsterPoolList[index].GetObject(); //Ǯ���� ��ť

        activatedMonsterList.Add(monster); //Ȱ��ȭ�Ǿ��ִ� ���� ����Ʈ�� �־���

        monster.transform.SetParent(field); //Ȱ��ȭ ���� �ʱ�ȭ
        monster.transform.position = pos; //���� ��ġ ����
        monster.gameObject.SetActive(true);

        monster.ResetMonster(); //���͸� ��ȿ�� ���·� �缳��

        InGameManager.Instance.MonsterList.Add(monster);

        return monster;
    }
    public NormalMonster GetMonster(Vector3 pos, int index, bool isRush, Vector3 rushDir, float distance) //Ǯ���� ���͸� �������� �Լ�
    {
        if (!monsterPoolList[index].IsValid())
        {
            foreach (var item in monsterPoolList[index].Pool)
            {
                item.InitMonsterData();
                item.InitDamageUIContainer();
                item.ReturnIndex = index;
            }
        }

        NormalMonster monster = monsterPoolList[index].GetObject(); //Ǯ���� ��ť

        activatedMonsterList.Add(monster); //Ȱ��ȭ�Ǿ��ִ� ���� ����Ʈ�� �־���

        monster.transform.SetParent(field); //Ȱ��ȭ ���� �ʱ�ȭ
        monster.transform.position = pos; //���� ��ġ ����
        monster.gameObject.SetActive(true);

        monster.ResetMonster(isRush, rushDir, distance); //���͸� ��ȿ�� ���·� �缳��

        InGameManager.Instance.MonsterList.Add(monster);

        return monster;
    }
    public void ReturnMonster(NormalMonster monster, int index) //���͸� Ǯ�� �ǵ����� �Լ�
    {
        activatedMonsterList.Remove(monster); //Ȱ��ȭ�Ǿ��ִ� ���� ����Ʈ���� ���� ����

        monster.gameObject.SetActive(false); //Ǯ�� �ǵ����� ���� �ʱ�ȭ
        monster.transform.SetParent(transform);

        monsterPoolList[index].ReturnObject(monster);
    }
    private IEnumerator Co_SpawnMonster(int index)
    {
        yield return new WaitForSeconds(createStartTime[index]);
        isActive[index] = true;
        Vector3 rotDir;
        while(!InGameManager.Instance.bAppearBoss)
        {
            if (InGameManager.Instance.Timer > createEndTime[index]) break;
            if(activatedMonsterList.Count < 200)
            {
                if (!InGameManager.Instance.bTimeStop)
                {
                    for (int i = 0; i < createCount[index]; i++)
                    {
                        rotDir = Quaternion.Euler(0, Random.Range(0, 361), 0) * Vector3.forward; //������ ���� ����
                        GetMonster(InGameManager.Instance.Player.transform.position + rotDir.normalized * monsterSpawnDistance, index);
                    }
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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ExecuteWave(3);
        }
    }
    private IEnumerator Co_ExecuteRandomWave()
    {
        while(!InGameManager.Instance.bAppearBoss)
        {
            float timer = 0;
            while(timer < 120)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            ExecuteWave(GetRandomWeight());
        }
    }
    private void ExecuteWave(int index)
    {
        switch (index)
        {
            case 0:
                Debug.Log("���̺� ��ŵ");
                break;
            case 1:
                ExcuteWave_Swarm();
                break;
            case 2:
                ExcuteWave_Rush();
                break;
            case 3:
                ExcuteWave_Cartel();
                break;
            case 4:
                ExcuteWave_GiftBox();
                break;
            case 5:
                //ExcuteWave_Fight();
                break;
            default:
                Debug.LogError("Wave Index is Not Valid");
                break;
        }
    }
    private void ExcuteWave_Swarm()
    {
        int rand = Random.Range(0, 4);
        Vector3 dir = GetRandomDirection();

        float summonRadius = InGameManager.Instance.Player.Level / 2;
        Vector3 randomPos;
        
        for (int i = 0; i < InGameManager.Instance.Player.Level; i++)
        {
            randomPos = new Vector3(Random.Range(0, summonRadius), 0, Random.Range(0, summonRadius));
            GetMonster(InGameManager.Instance.Player.transform.position + dir * monsterSpawnDistance + randomPos, 0);
        }
    }
    private void ExcuteWave_Rush()
    {
        int rand = Random.Range(10, 16);
        Vector3 dir = GetRandomDirection();

        Vector3 summonPos = InGameManager.Instance.Player.transform.position + dir * monsterSpawnDistance;
        int pivot = rand / 2;

        Vector3 sortDir = Vector3.zero;
        if(dir == Vector3.forward || dir == Vector3.back)
        {
            sortDir = Vector3.right;
            summonPos.x += pivot;
        }
        else
        {
            sortDir = Vector3.forward;
            summonPos.z += pivot;
        }

        for (int i = 0; i < rand; i++)
        {
            GetMonster(summonPos - sortDir * i, 0, true, dir * -1, 20);
        }
    }
    private void ExcuteWave_Cartel()
    {
        cartel.ActiveCartel(5f);
    }
    private void ExcuteWave_GiftBox()
    {

    }
    private void ExcuteWave_Fight()
    {

    }
    private Vector3 GetRandomDirection()
    {
        int rand = Random.Range(0, 4);
        Vector3 dir = Vector3.zero;
        switch (rand)
        {
            case 0:
                dir = Vector3.forward;
                break;
            case 1:
                dir = Vector3.back;
                break;
            case 2:
                dir = Vector3.left;
                break;
            case 3:
                dir = Vector3.right;
                break;
            default:
                Debug.LogError("Random Value Error");
                break;
        }
        return dir;
    }
    private int GetRandomWeight()
    {
        int rand = Random.Range(0, waveWeightArray.Sum());

        int weight = waveWeightArray[0];

        for (int i = 0; i < waveWeightArray.Length; i++)
        {
            if(rand < weight)
            {
                return i;
            }
            else
            {
                weight += waveWeightArray[i];
            }
        }

        return waveWeightArray.Length - 1;
    }
}
