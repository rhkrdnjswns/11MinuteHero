using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour //���͸� Ǯ��, �����ϴ� Ŭ����. ���� Ǯ�� ������ ���� �����ʿ����� �ʿ��ϱ� ������ ���� �̱������� �� �ʿ䰡 ����.
{
    [SerializeField] private GameObject[] monsterPrefabArray; //���� ������ �迭
    private List<Queue<Monster>> monsterQueue = new List<Queue<Monster>>(); //Ǯ
    private Transform field; //������ ������ transform parent�� �� ����

    private List<Monster> activatedMonsterList = new List<Monster>();

    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnStartDelay;

    [Range(25, 50)]
    [SerializeField] private float monsterSpawnDistance;

    private Vector3 rotDir;
    private WaitForSeconds monsterSpawnInterval;
    public List<Monster> ActivatedMonsterList { get => activatedMonsterList; }

    public int summonCountForTest; //Ǯ�� ������ ���� ��
    private void Awake()
    { 
        CreateNewMonster(summonCountForTest);
        monsterSpawnInterval = new WaitForSeconds(spawnInterval);
        field = GameObject.Find("Field").transform;
    }
    private void CreateNewMonster(int count) //count��ŭ ���� ����
    {
        for (int i = 0; i < monsterPrefabArray.Length; i++)
        {
            monsterQueue.Add(new Queue<Monster>());
            CreateMonster(i);
        }
    }
    private void CreateMonster(int index) //���� ����
    {
        for(int i = 0; i < summonCountForTest; i++)
        {
            GameObject obj = Instantiate(monsterPrefabArray[index]);
            obj.SetActive(false);
            obj.transform.SetParent(transform); //���� ���� �� ��Ȱ��ȭ,����Ǯ�� �θ�� ����

            Monster m = obj.GetComponent<Monster>();
            m.name = m.name + index;
            m.InitDamageUIContainer();
            m.ReturnIndex = index;

            monsterQueue[index].Enqueue(m);
        }
    }
    public Monster GetMonster(Vector3 pos) //Ǯ���� ���͸� �������� �Լ�
    {
        int rand = Random.Range(0, monsterPrefabArray.Length);
        if (monsterQueue[rand].Count <= 0) //Ǯ�� ���Ͱ� ������ Ǯ�� ���� ����
        {
            CreateNewMonster(summonCountForTest);
        }
        Monster monster = monsterQueue[rand].Dequeue(); //Ǯ���� ��ť

        activatedMonsterList.Add(monster); //Ȱ��ȭ�Ǿ��ִ� ���� ����Ʈ�� �־���

        monster.transform.SetParent(field); //Ȱ��ȭ ���� �ʱ�ȭ
        monster.transform.position = pos; //���� ��ġ ����
        monster.gameObject.SetActive(true);

        monster.ResetMonster(); //���͸� ��ȿ�� ���·� �缳��

        InGameManager.Instance.MonsterList.Add(monster);

        return monster;
    }
    public Monster GetMonster(Vector3 pos, int index) //Ǯ���� ���͸� �������� �Լ�
    {
        if (monsterQueue[index].Count <= 0) //Ǯ�� ���Ͱ� ������ Ǯ�� ���� ����
        {
            CreateNewMonster(summonCountForTest);
        }
        Monster monster = monsterQueue[index].Dequeue(); //Ǯ���� ��ť

        activatedMonsterList.Add(monster); //Ȱ��ȭ�Ǿ��ִ� ���� ����Ʈ�� �־���

        monster.transform.SetParent(field); //Ȱ��ȭ ���� �ʱ�ȭ
        monster.transform.position = pos; //���� ��ġ ����
        monster.gameObject.SetActive(true);

        monster.ResetMonster(); //���͸� ��ȿ�� ���·� �缳��

        InGameManager.Instance.MonsterList.Add(monster);

        return monster;
    }
    public void ReturnMonster(Monster monster, int index) //���͸� Ǯ�� �ǵ����� �Լ�
    {
        activatedMonsterList.Remove(monster); //Ȱ��ȭ�Ǿ��ִ� ���� ����Ʈ���� ���� ����

        monster.gameObject.SetActive(false); //Ǯ�� �ǵ����� ���� �ʱ�ȭ
        monster.transform.SetParent(transform);

        monsterQueue[index].Enqueue(monster);
    }
    private IEnumerator Co_SpawnMonster()
    {
        yield return new WaitForSeconds(spawnStartDelay);

        while(!InGameManager.Instance.bAppearBoss)
        {
            rotDir = Quaternion.Euler(0, Random.Range(0, 361), 0) * Vector3.forward; //������ ���� ����
            yield return monsterSpawnInterval;
            //�÷��̾� ��ġ���� ������ �������� monsterSpawnDistance �Ÿ��� ���� ����
            GetMonster(InGameManager.Instance.Player.transform.position + rotDir.normalized * monsterSpawnDistance);
        }
    }
}
