using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour //���͸� Ǯ��, �����ϴ� Ŭ����. ���� Ǯ�� ������ ���� �����ʿ����� �ʿ��ϱ� ������ ���� �̱������� �� �ʿ䰡 ����.
{
    [SerializeField] private GameObject[] monsterPrefabArray; //���� ������ �迭
    private Queue<Monster> monsterQueue = new Queue<Monster>(); //Ǯ
    private Transform field; //������ ������ transform parent�� �� ����

    private List<Monster> activatedMonsterList = new List<Monster>();
    public List<Monster> ActivatedMonsterList { get => activatedMonsterList; }

    public int summonCountForTest; //Ǯ�� ������ ���� ��
    private void Awake()
    { 
        CreateNewMonster(summonCountForTest);
        field = GameObject.Find("Field").transform;
    }
    private void CreateNewMonster(int count) //count��ŭ ���� ����
    {
        for (int i = 0; i < count; i++)
        {
            CreateMonster(i);
        }
    }
    private void CreateMonster(int index) //���� ����
    {
        GameObject obj = Instantiate(monsterPrefabArray[Random.Range(0, monsterPrefabArray.Length)]);
        obj.SetActive(false);
        obj.transform.SetParent(transform); //���� ���� �� ��Ȱ��ȭ,����Ǯ�� �θ�� ����

        Monster m = obj.GetComponent<Monster>();
        m.name = m.name + index;
        m.InitDamageUIContainer();

        monsterQueue.Enqueue(m);
    }
    public Monster GetMonster(Vector3 pos) //Ǯ���� ���͸� �������� �Լ�
    {
        if (monsterQueue.Count <= 0) //Ǯ�� ���Ͱ� ������ Ǯ�� ���� ����
        {
            CreateNewMonster(summonCountForTest);
        }
        Monster monster = monsterQueue.Dequeue(); //Ǯ���� ��ť

        activatedMonsterList.Add(monster); //Ȱ��ȭ�Ǿ��ִ� ���� ����Ʈ�� �־���

        monster.transform.SetParent(field); //Ȱ��ȭ ���� �ʱ�ȭ
        monster.transform.position = pos; //���� ��ġ ����
        monster.gameObject.SetActive(true);

        monster.ResetMonster(); //���͸� ��ȿ�� ���·� �缳��

        InGameManager.Instance.MonsterList.Add(monster);

        return monster;
    }
    public void ReturnMonster(Monster monster) //���͸� Ǯ�� �ǵ����� �Լ�
    {
        activatedMonsterList.Remove(monster); //Ȱ��ȭ�Ǿ��ִ� ���� ����Ʈ���� ���� ����

        monster.gameObject.SetActive(false); //Ǯ�� �ǵ����� ���� �ʱ�ȭ
        monster.transform.SetParent(transform);

        monsterQueue.Enqueue(monster);
    }
}
