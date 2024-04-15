using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour //몬스터를 풀링, 관리하는 클래스. 몬스터 풀의 참조는 몬스터 스포너에서만 필요하기 때문에 굳이 싱글턴으로 둘 필요가 없다.
{
    [SerializeField] private GameObject[] monsterPrefabArray; //몬스터 프리팹 배열
    private List<Queue<NormalMonster>> monsterQueue = new List<Queue<NormalMonster>>(); //풀
    private Transform field; //생성된 몬스터의 transform parent가 될 참조

    private List<NormalMonster> activatedMonsterList = new List<NormalMonster>();

    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnStartDelay;

    [Range(25, 50)]
    [SerializeField] private float monsterSpawnDistance;

    private Vector3 rotDir;
    private WaitForSeconds monsterSpawnInterval;
    public List<NormalMonster> ActivatedMonsterList { get => activatedMonsterList; }

    public int summonCountForTest; //풀에 생성할 몬스터 수
    private void Awake()
    { 
        CreateNewMonster(summonCountForTest);
        monsterSpawnInterval = new WaitForSeconds(spawnInterval);
        field = GameObject.Find("Field").transform;
    }
    private void CreateNewMonster(int count) //count만큼 몬스터 생성
    {
        for (int i = 0; i < monsterPrefabArray.Length; i++)
        {
            monsterQueue.Add(new Queue<NormalMonster>());
            CreateMonster(i);
        }
    }
    private void CreateMonster(int index) //몬스터 생성
    {
        for(int i = 0; i < summonCountForTest; i++)
        {
            GameObject obj = Instantiate(monsterPrefabArray[index]);
            obj.SetActive(false);
            obj.transform.SetParent(transform); //몬스터 생성 후 비활성화,몬스터풀을 부모로 해줌

            NormalMonster m = obj.GetComponent<NormalMonster>();
            m.InitDamageUIContainer();
            m.ReturnIndex = index;

            monsterQueue[index].Enqueue(m);
        }
    }
    public NormalMonster GetMonster(Vector3 pos) //풀에서 몬스터를 꺼내오는 함수
    {
        int rand = Random.Range(0, monsterPrefabArray.Length);
        if (monsterQueue[rand].Count <= 0) //풀에 몬스터가 없으면 풀에 새로 생성
        {
            CreateMonster(rand);
        }
        NormalMonster monster = monsterQueue[rand].Dequeue(); //풀에서 디큐

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
        if (monsterQueue[index].Count <= 0) //풀에 몬스터가 없으면 풀에 새로 생성
        {
            CreateMonster(index);
        }
        NormalMonster monster = monsterQueue[index].Dequeue(); //풀에서 디큐

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

        monsterQueue[index].Enqueue(monster);
    }
    private IEnumerator Co_SpawnMonster()
    {
        yield return new WaitForSeconds(spawnStartDelay);

        while(!InGameManager.Instance.bAppearBoss)
        {
            rotDir = Quaternion.Euler(0, Random.Range(0, 361), 0) * Vector3.forward; //랜덤한 방향 설정
            yield return monsterSpawnInterval;
            //플레이어 위치에서 랜덤한 방향으로 monsterSpawnDistance 거리에 몬스터 생성
            GetMonster(InGameManager.Instance.Player.transform.position + rotDir.normalized * monsterSpawnDistance);
        }
    }
}
