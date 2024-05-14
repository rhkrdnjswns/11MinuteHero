using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour //���͸� Ǯ��, �����ϴ� Ŭ����. ���� Ǯ�� ������ ���� �����ʿ����� �ʿ��ϱ� ������ ���� �̱������� �� �ʿ䰡 ����.
{
    [SerializeField] private GameObject[] monsterPrefabArray; //���� ������ �迭
    private Transform field; //������ ������ transform parent�� �� ����

    private List<ObjectPool<NormalMonster>> monsterPoolList = new List<ObjectPool<NormalMonster>>();
    private List<NormalMonster> activatedMonsterList = new List<NormalMonster>();

    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnStartDelay;

    [Range(25, 50)]
    [SerializeField] private float monsterSpawnDistance;

    private Vector3 rotDir;
    private WaitForSeconds monsterSpawnInterval;
    public List<NormalMonster> ActivatedMonsterList { get => activatedMonsterList; }

    public int summonCountForTest; //Ǯ�� ������ ���� ��
    private void Awake()
    { 
        CreateNewMonster();
        monsterSpawnInterval = new WaitForSeconds(spawnInterval);
        field = GameObject.Find("Field").transform;
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
        monsterPoolList[index].IsValid();

        NormalMonster monster = monsterPoolList[index].GetObject(); //Ǯ���� ��ť

        activatedMonsterList.Add(monster); //Ȱ��ȭ�Ǿ��ִ� ���� ����Ʈ�� �־���

        monster.transform.SetParent(field); //Ȱ��ȭ ���� �ʱ�ȭ
        monster.transform.position = pos; //���� ��ġ ����
        monster.gameObject.SetActive(true);

        monster.ResetMonster(); //���͸� ��ȿ�� ���·� �缳��

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
