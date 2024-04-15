using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRedGolemHard : BRedGolemNormal
{
    [SerializeField] private GameObject earthQuakeStonePrefab;
    protected Queue<CRedGolemStone> earthQuakeStoneQueue = new Queue<CRedGolemStone>();
    private float summonedEarthQuakeStonePosY = 0f;

    [SerializeField] private List<Decal> hpEventEarthQuakeDecalList;
    [SerializeField] private List<ParticleSystem> earthQuakeParticleList;

    public override void InitBoss()
    {
        base.InitBoss();
        foreach (var item in earthQuakeParticleList)
        {
            item.transform.SetParent(null);
        }
    }
    protected override void PlayHpEvent(int index)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(Co_HpEvent_Release());
                break;
            case 1:
                StartCoroutine(Co_HpEvent_EarthQuake());
                break;
            default:
                Debug.LogError($"{this}\nPlayerHpEvent : Index Out Range");
                break;
        }
    }
    private IEnumerator Co_HpEvent_EarthQuake()
    {
        bHpEvent = false;
        float spacing;
        int rand;
        while (true)
        {
            yield return new WaitForSeconds(5f);

            spacing = bossAreaHeight / hpEventEarthQuakeDecalList.Count;
            rand = Random.Range(0, 2);

            for (int i = 0; i < hpEventEarthQuakeDecalList.Count; i++)
            {
                hpEventEarthQuakeDecalList[i].transform.SetParent(null);

                hpEventEarthQuakeDecalList[i].transform.position = appearPos + (rand == 0 ? Vector3.back : Vector3.forward) * (((bossAreaHeight - 5) / 2) - spacing * i);
                hpEventEarthQuakeDecalList[i].transform.rotation = Quaternion.Euler(90, 0, 0);

                ParticleSystem.ShapeModule shape = earthQuakeParticleList[i].shape;
                shape.scale = new Vector3(bossAreaWidth, 1, 4);
                earthQuakeParticleList[i].transform.position = appearPos + (rand == 0 ? Vector3.back : Vector3.forward) * (((bossAreaHeight - 5) / 2) - spacing * i);
                earthQuakeParticleList[i].transform.rotation = Quaternion.Euler(90, 0, 0);

                if (i < hpEventEarthQuakeDecalList.Count - 1)
                {
                    StartCoroutine(hpEventEarthQuakeDecalList[i].Co_ActiveDecal(new Vector3(bossAreaWidth, 4, 1), 2f));
                }
                else
                {
                    yield return StartCoroutine(hpEventEarthQuakeDecalList[i].Co_ActiveDecal(new Vector3(bossAreaWidth, 4, 1), 2f));
                }
            }
            for (int i = 0; i < hpEventEarthQuakeDecalList.Count; i++)
            {
                earthQuakeParticleList[i].Play();
                attackInSquareUtility.AttackLayerInSquare(attackInSquareUtility.GetLayerInSquare(hpEventEarthQuakeDecalList[i].transform.position, new Vector3(bossAreaWidth / 2, 1, 2), Quaternion.identity), 20);
                hpEventEarthQuakeDecalList[i].InActiveDecal(transform);
            }
            StartCoroutine(cameraUtility.Co_ShakeCam(0.2f, 1, 0.1f));

            yield return new WaitForSeconds(5f);

            spacing = bossAreaWidth / hpEventEarthQuakeDecalList.Count;
            rand = Random.Range(0, 2);

            for (int i = 0; i < hpEventEarthQuakeDecalList.Count; i++)
            {
                hpEventEarthQuakeDecalList[i].transform.SetParent(null);

                hpEventEarthQuakeDecalList[i].transform.position = appearPos + (rand == 0 ? Vector3.left : Vector3.right) * (((bossAreaWidth - 5) / 2) - spacing * i);
                hpEventEarthQuakeDecalList[i].transform.rotation = Quaternion.Euler(90, 0, -90);

                ParticleSystem.ShapeModule shape = earthQuakeParticleList[i].shape;
                shape.scale = new Vector3(bossAreaHeight, 1, 4);
                earthQuakeParticleList[i].transform.position = appearPos + (rand == 0 ? Vector3.left : Vector3.right) * (((bossAreaWidth - 5) / 2) - spacing * i);
                earthQuakeParticleList[i].transform.rotation = Quaternion.Euler(90, 0, -90);

                if (i < hpEventEarthQuakeDecalList.Count - 1)
                {
                    StartCoroutine(hpEventEarthQuakeDecalList[i].Co_ActiveDecal(new Vector3(bossAreaHeight, 4, 1), 2f));
                }
                else
                {
                    yield return StartCoroutine(hpEventEarthQuakeDecalList[i].Co_ActiveDecal(new Vector3(bossAreaHeight, 4, 1), 2f));
                }
            }
            for (int i = 0; i < hpEventEarthQuakeDecalList.Count; i++)
            {
                earthQuakeParticleList[i].Play();
                attackInSquareUtility.AttackLayerInSquare(attackInSquareUtility.GetLayerInSquare(hpEventEarthQuakeDecalList[i].transform.position, new Vector3(2, 1, bossAreaHeight / 2), Quaternion.identity), 20);
                hpEventEarthQuakeDecalList[i].InActiveDecal(transform);
            }
            StartCoroutine(cameraUtility.Co_ShakeCam(0.2f, 1, 0.1f));
        }
    }
    protected override void SetSummonedStonePosY()
    {
        base.SetSummonedStonePosY();
        summonedEarthQuakeStonePosY = 0f;
    }
    protected override void InitStoneQueue()
    {
        base.InitStoneQueue();
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = Instantiate(earthQuakeStonePrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;

            CRedGolemStone stone = obj.GetComponent<CRedGolemStone>().SetReference(transform);
            stone.InitDamageUIContainer();
            earthQuakeStoneQueue.Enqueue(stone);
        }
    }
    public override void SummonStone(Vector3 pos)
    {
        int rand = Random.Range(1, 101);
        float posY;
  
        if (rand <= 60)
        {
            if (stoneQueue.Count == 0) InitStoneQueue();
            stoneRef = stoneQueue.Dequeue();
            posY = summonedStonePosY;
        }
        else if (rand <= 90)
        {
            if (indestructibleStoneQueue.Count == 0) InitStoneQueue();
            stoneRef = indestructibleStoneQueue.Dequeue();
            posY = summonedIndestructibleStonePosY;
        }
        else
        {
            if (earthQuakeStoneQueue.Count == 0) InitStoneQueue();
            stoneRef = earthQuakeStoneQueue.Dequeue();
            posY = summonedEarthQuakeStonePosY;
        }

        if (bReturnStone)
        {
            spareStoneList.Add(stoneRef);
        }
        else
        {
            stoneList.Add(stoneRef);
        }
        stoneSummonPos = pos;
        stoneSummonPos.y = summonedStonePosY;

        stoneRef.transform.SetParent(null);
        stoneRef.transform.position = stoneSummonPos;
        stoneRef.gameObject.SetActive(true);
        stoneRef.ResetStatus();
    }
    protected override void ReturnStoneByLevel(CRedGolemStone redGolemStone, int stoneLevel)
    {
        base.ReturnStoneByLevel(redGolemStone, stoneLevel);
        if (stoneLevel == 2)
        {
            earthQuakeStoneQueue.Enqueue(redGolemStone);
        }
    }
}
