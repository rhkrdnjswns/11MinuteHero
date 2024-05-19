using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Monster
{
    protected BehaviorTree behaviorTree;
    [Range(20, 60)]
    [SerializeField] protected float bossAreaWidth;
    [Range(20, 60)]
    [SerializeField] protected float bossAreaHeight;
    [SerializeField] private GameObject bossAreaPrefab;

    protected CameraUtility cameraUtility;
    [SerializeField] protected List<Decal> decalList = new List<Decal>();

    [SerializeField] protected int[] hpEventArray;
    private int hpEventIndex;
    protected bool bHpEvent;

    public bool IsActivate { get; private set; }
    protected GameObject modelObject;
    protected abstract void PlayHpEvent(int index);
    protected abstract void InitBehaviorTree();
    public virtual void InitBoss()
    {
        modelObject = transform.GetChild(0).gameObject;
        InitDamageUIContainer(modelObject.transform.localScale.y);
        cameraUtility = Camera.main.GetComponent<CameraUtility>();
        InitBehaviorTree();
        InGameManager.Instance.DGameOver += StopAllCoroutines;
    }
    public float GetStartAnimPlayTime()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }
    public virtual void ActiveBoss()
    {
        StartCoroutine(Co_ExcuteBehaviorTree());
        IsActivate = true;
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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            bHpEvent = true;
            PlayHpEvent(1);
        }
    }
    private IEnumerator Co_ExcuteBehaviorTree()
    {
        while (!IsDie)
        {
            yield return null;
            if (bHpEvent) continue;
            behaviorTree.Execute();
        }
    }
    public override void Hit(float damage)
    {
        if (bHpEvent) return;
        base.Hit(damage);
        InGameManager.Instance.BossUIManager.UpdateBossHpBar(currentHp / maxHp);
        if(currentHp <= 0 && !IsDie)
        {
            InGameManager.Instance.DGameOver();
            StartCoroutine(Co_Die());
            return;
        }
        if (hpEventIndex >= hpEventArray.Length) return;
        if((currentHp / maxHp * 100) <= hpEventArray[hpEventIndex])
        {
            bHpEvent = true;
            PlayHpEvent(hpEventIndex++);
        }
        
    }
    protected IEnumerator Co_Die()
    {
        IsDie = true;
        animator.SetTrigger(ConstDefine.TRIGGER_DIE);
        yield return new WaitForSeconds(1.5f);

        InGameManager.Instance.StartCoroutine(InGameManager.Instance.Co_ShowResultPopup(true));
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
