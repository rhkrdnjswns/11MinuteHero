using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour //몬스터를 풀링, 관리하는 클래스. 몬스터 풀의 참조는 몬스터 스포너에서만 필요하기 때문에 굳이 싱글턴으로 둘 필요가 없다.
{
    [SerializeField] private GameObject[] monsterPrefabArray; //몬스터 프리팹 배열
    private Queue<Monster> monsterQueue = new Queue<Monster>(); //풀
    private Transform field; //생성된 몬스터의 transform parent가 될 참조

    private List<Monster> activatedMonsterList = new List<Monster>();
    public List<Monster> ActivatedMonsterList { get => activatedMonsterList; }

    public int summonCountForTest; //풀에 생성할 몬스터 수
    private void Awake()
    { 
        CreateNewMonster(summonCountForTest);
        field = GameObject.Find("Field").transform;
    }
    private void CreateNewMonster(int count) //count만큼 몬스터 생성
    {
        for (int i = 0; i < count; i++)
        {
            CreateMonster(i);
        }
    }
    private void CreateMonster(int index) //몬스터 생성
    {
        GameObject obj = Instantiate(monsterPrefabArray[Random.Range(0, monsterPrefabArray.Length)]);
        obj.SetActive(false);
        obj.transform.SetParent(transform); //몬스터 생성 후 비활성화,몬스터풀을 부모로 해줌

        Monster m = obj.GetComponent<Monster>();
        m.name = m.name + index;
        m.InitDamageUIContainer();

        monsterQueue.Enqueue(m);
    }
    public Monster GetMonster(Vector3 pos) //풀에서 몬스터를 꺼내오는 함수
    {
        if (monsterQueue.Count <= 0) //풀에 몬스터가 없으면 풀에 새로 생성
        {
            CreateNewMonster(summonCountForTest);
        }
        Monster monster = monsterQueue.Dequeue(); //풀에서 디큐

        activatedMonsterList.Add(monster); //활성화되어있는 몬스터 리스트에 넣어줌

        monster.transform.SetParent(field); //활성화 관련 초기화
        monster.transform.position = pos; //몬스터 위치 설정
        monster.gameObject.SetActive(true);

        monster.ResetMonster(); //몬스터를 유효한 상태로 재설정

        InGameManager.Instance.MonsterList.Add(monster);

        return monster;
    }
    public void ReturnMonster(Monster monster) //몬스터를 풀로 되돌리는 함수
    {
        activatedMonsterList.Remove(monster); //활성화되어있는 몬스터 리스트에서 몬스터 삭제

        monster.gameObject.SetActive(false); //풀로 되돌리기 관련 초기화
        monster.transform.SetParent(transform);

        monsterQueue.Enqueue(monster);
    }
}
