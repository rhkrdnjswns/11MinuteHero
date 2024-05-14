using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour //몬스터를 풀링, 관리하는 클래스. 몬스터 풀의 참조는 몬스터 스포너에서만 필요하기 때문에 굳이 싱글턴으로 둘 필요가 없다.
{
    [SerializeField] private GameObject[] monsterPrefabArray; //몬스터 프리팹 배열
    private Transform field; //생성된 몬스터의 transform parent가 될 참조

    private List<ObjectPool<NormalMonster>> monsterPoolList = new List<ObjectPool<NormalMonster>>();
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
        CreateNewMonster();
        monsterSpawnInterval = new WaitForSeconds(spawnInterval);
        field = GameObject.Find("Field").transform;
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
        monsterPoolList[index].IsValid();

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
