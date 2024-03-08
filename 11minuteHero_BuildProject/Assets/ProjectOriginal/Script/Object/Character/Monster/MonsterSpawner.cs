using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour //���͸� ������ ��ġ�� �������ִ� Ŭ����
{
    [SerializeField] private float spawnInterval; //��ȯ ����
    [SerializeField] private float startDelay; //���� ���� �� ��ȯ������ ������

    private Transform[] spawnPointArray = new Transform[12]; // ��ȯ�� ��ġ
    private int index;
    private void Awake()
    {
        for (int i = 0; i < spawnPointArray.Length; i++) //�迭 �ʱ�ȭ
        {
            spawnPointArray[i] = transform.GetChild(i);
        }
    }
    private void Start()
    {
        InGameManager.Instance.DGameOver += StopAllCoroutines; //���ӿ��� �� �ڷ�ƾ �۵� ���߰� ��������Ʈ�� �߰�
        StartCoroutine(Co_SpawnMonster());
    }
    private IEnumerator Co_SpawnMonster() //��ȯ ���ݸ��� ���͸� ��ȯ�ϴ� �ڷ�ƾ
    {
        yield return new WaitForSeconds(startDelay);
        while (true)
        {
            if (InGameManager.Instance.MonsterList.Count < 100)
            {
                InGameManager.Instance.MonsterPool.GetMonster(spawnPointArray[Random.Range(0, spawnPointArray.Length)].position); //��ȯ�� ��ġ �� ������ ��ġ���� ��ȯ
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
