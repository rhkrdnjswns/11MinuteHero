using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected bool isGet; //ȹ���� ���������� üũ
    protected Coroutine upAndDownAnim; //�ڷ�ƾ ����
    protected Coroutine stopAnim; //�ڷ�ƾ ����
    [SerializeField] private EItemID itemID;

    private SpriteRenderer spriteRenderer;
    private WaitUntil waitUntilTimeStopFalse;
    public EItemID ItemID { get => itemID; }
    protected abstract void Interaction();
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public void InitItem(Vector3 pos) //������ ���� �� �ʱ�ȭ
    {
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        gameObject.SetActive(true);
        isGet = false;

        if (InGameManager.Instance.bTimeStop)
        {
            if(waitUntilTimeStopFalse == null) waitUntilTimeStopFalse = new WaitUntil(() => !InGameManager.Instance.bTimeStop);
            StartCoroutine(Co_WaitUntilTimeStopFalse());
            return;
        }
        upAndDownAnim = StartCoroutine(TweeningUtility.UpAndDown(0.5f, 0.3f, transform)); //����ƽ �ڷ�ƾ�� ������ ������ ���� (�ڷ�ƾ �ߴ� ����� ����ϱ� ����)
    }
    protected void ReturnItem()
    {
        InGameManager.Instance.ItemManager.ReturnItem(this, itemID);
    }
    protected virtual IEnumerator Co_ItemAnimation(Vector3 direction)
    {
        if(upAndDownAnim != null)
        {
            StopCoroutine(upAndDownAnim);
        }
        transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
        direction.y = 0; //���� ������ y���� 0���� �ؼ� y�� �̵� ����
        float timer = 0;

        while (Vector3.Distance(transform.position, InGameManager.Instance.Player.transform.position + Vector3.up * 0.3f) > 0.2f) //�÷��̾� ��ġ�� �������� ���� �ִϸ��̼� ����
        {
            timer += Time.deltaTime;
            transform.position += (InGameManager.Instance.Player.transform.position + Vector3.up * 0.3f - transform.position).normalized * (10 * (1 + timer)) * Time.deltaTime;
            yield return null;
        }
        Interaction();
    }
    public virtual void GetExpItem()
    {
        return;
    }
    public void StopAnim(float time)
    {
        if(stopAnim != null)
        {
            StopCoroutine(stopAnim);
        }
        stopAnim = StartCoroutine(Co_StopAnim(time));
    }
    private IEnumerator Co_WaitUntilTimeStopFalse()
    {
        yield return waitUntilTimeStopFalse;
        upAndDownAnim = StartCoroutine(TweeningUtility.UpAndDown(0.5f, 0.3f, transform));
    }
    private IEnumerator Co_StopAnim(float time)
    {
        if(upAndDownAnim != null)
        {
            StopCoroutine(upAndDownAnim);
        }
        float timer = 0;
        while(timer < time)
        {
            timer += Time.deltaTime;
            yield return null;
        }        
        if(!isGet) upAndDownAnim = StartCoroutine(TweeningUtility.UpAndDown(0.5f, 0.3f, transform));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isGet) return;
        if (other.CompareTag(ConstDefine.TAG_PLAYER))
        {
            StartCoroutine(Co_ItemAnimation(-(InGameManager.Instance.Player.transform.position - transform.position).normalized));
            isGet = true;
        }
    }
}
