using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour //몬스터를 랜덤한 위치에 스폰해주는 클래스
{
    [SerializeField] private float spawnInterval; //소환 간격
    [SerializeField] private float startDelay; //게임 시작 후 소환까지의 딜레이

    private Transform[] spawnPointArray = new Transform[12]; // 소환될 위치
    private int index;
    private void Awake()
    {
        for (int i = 0; i < spawnPointArray.Length; i++) //배열 초기화
        {
            spawnPointArray[i] = transform.GetChild(i);
        }
    }
    private void Start()
    {
        InGameManager.Instance.DGameOver += StopAllCoroutines; //게임오버 시 코루틴 작동 멈추게 델리게이트에 추가
        StartCoroutine(Co_SpawnMonster());
    }
    private IEnumerator Co_SpawnMonster() //소환 간격마다 몬스터를 소환하는 코루틴
    {
        yield return new WaitForSeconds(startDelay);
        while (true)
        {
            if (InGameManager.Instance.MonsterList.Count < 100)
            {
                InGameManager.Instance.MonsterPool.GetMonster(spawnPointArray[Random.Range(0, spawnPointArray.Length)].position); //소환될 위치 중 랜덤한 위치에서 소환
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
