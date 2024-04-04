using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Character
{
    protected BehaviorTree behaviorTree;
    [Range(20, 60)]
    [SerializeField] private float bossAreaWidth;
    [Range(20, 60)]
    [SerializeField] private float bossAreaHeight;
    [SerializeField] private GameObject bossAreaPrefab;

    [SerializeField] protected List<Decal> decalList = new List<Decal>();

    public float GetStartAnimPlayTime()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }
    protected abstract void InitBehaviorTree();
    public void InitBoss()
    {
        InitBehaviorTree();
        StartCoroutine(Co_ExcuteBehaviorTree());
    }
    public void CreateBossArea()
    {
        for (int i = -1; i < 2; i+=2)
        {
            GameObject obj = Instantiate(bossAreaPrefab);

            obj.transform.SetParent(transform);
            obj.transform.localPosition= Vector3.zero;
            obj.transform.SetParent(null);
            obj.transform.position += Vector3.forward * i * (bossAreaHeight / 2);
            obj.transform.localScale = new Vector3(bossAreaWidth, 1, 1); 
            if (i < 0) obj.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        for (int i = -1; i < 2; i += 2)
        {
            GameObject obj = Instantiate(bossAreaPrefab);

            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.SetParent(null);
            obj.transform.position += Vector3.right * i * (bossAreaWidth / 2);
            obj.transform.localScale = new Vector3(bossAreaHeight, 1, 1);
            if (i < 0) obj.transform.rotation = Quaternion.Euler(0, 270, 0);
            else obj.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    private IEnumerator Co_ExcuteBehaviorTree()
    {
        while (true)
        {
            behaviorTree.Execute();
            yield return null;
        }
    }
    public override void Hit(float damage)
    {
        base.Hit(damage);
        InGameManager.Instance.BossUIManager.UpdateBossHpBar(currentHp / maxHp);
    }
    protected virtual void IncreaseHp(float value)
    {
        currentHp += value;
        if (currentHp > maxHp) currentHp = maxHp;
        InGameManager.Instance.BossUIManager.UpdateBossHpBar(currentHp / maxHp);
    }
    public override void KnockBack(float speed, float duration)
    {
        return;
    }
    public override void KnockBack(float speed, float duration, Vector3 direction)
    {
        return;
    }
}
