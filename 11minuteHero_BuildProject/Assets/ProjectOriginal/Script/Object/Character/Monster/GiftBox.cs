using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : Monster
{
    public Transform mesh;
    public Transform decal;
    public Mimic mimic;
    public BoxCollider boxCollider;
    
    private WaitForSeconds activeDelay = new WaitForSeconds(1f);
    public void InitGiftBox()
    {
        InitDamageUIContainer();
        mimic.InitDamageUIContainer();
        mimic.InitMonsterData();
    }
    public void ActiveGiftBox()
    {
        Vector3 summonPos = InGameManager.Instance.Player.transform.position;
        summonPos.x += Random.Range(-3, 4);
        summonPos.z += Random.Range(-3, 4);
        transform.position = summonPos;

        StartCoroutine(Co_ActiveGiftBox());
    }
    private IEnumerator Co_ActiveGiftBox()
    {
        decal.gameObject.SetActive(true);
        yield return activeDelay;
        decal.gameObject.SetActive(false);

        IsDie = false;
        boxCollider.enabled = true;
        currentHp = maxHp;
        transform.LookAt(InGameManager.Instance.Player.transform);
        mesh.gameObject.SetActive(true);
    }
    public override void Hit(float damage)
    {
        base.Hit(damage);
        if(IsDie)
        {
            boxCollider.enabled = false;
            SummonRandomThing();
        }
    }
    private void SummonRandomThing()
    {
        int rand = Random.Range(0, 11);
        mesh.transform.gameObject.SetActive(false);
        if (rand < 8)
        {
            mimic.transform.localPosition = Vector3.zero;
            mimic.gameObject.SetActive(true);
            mimic.ResetMonster();
            InGameManager.Instance.MonsterList.Add(mimic);
        }
        else
        {
            InGameManager.Instance.ItemManager.GetItem(transform.position, GetRandomItem());
        }
    }
    private EItemID GetRandomItem()
    {
        int rand = Random.Range(4, 8);
        return (EItemID)rand;
    }
}
