using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected bool isGet; //획득한 아이템인지 체크
    protected Coroutine anim; //코루틴 참조
    [SerializeField] private EItemID itemID;

    private SpriteRenderer spriteRenderer;
    protected abstract void Interaction();
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public void InitItem(Vector3 pos) //아이템 생성 시 초기화
    {
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        gameObject.SetActive(true);
        isGet = false;

        anim = StartCoroutine(TweeningUtility.UpAndDown(0.5f, 0.3f, transform)); //스태틱 코루틴을 열거자 변수로 참조 (코루틴 중단 기능을 사용하기 위해)
    }
    protected void ReturnItem()
    {
        InGameManager.Instance.ItemManager.ReturnItem(this, itemID);
    }
    protected virtual IEnumerator Co_ItemAnimation(Vector3 direction)
    {
        if(anim != null)
        {
            StopCoroutine(anim);
        }
        transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
        direction.y = 0; //방향 벡터의 y값을 0으로 해서 y축 이동 제한
        float timer = 0;

        while (Vector3.Distance(transform.position, InGameManager.Instance.Player.transform.position + Vector3.up * 0.3f) > 0.2f) //플레이어 위치로 빨려가는 듯한 애니메이션 적용
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
        StartCoroutine(Co_StopAnim(time));
    }
    private IEnumerator Co_StopAnim(float time)
    {
        if(anim != null)
        {
            StopCoroutine(anim);
        }
        float timer = 0;
        while(timer < time)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        if(!isGet) anim = StartCoroutine(TweeningUtility.UpAndDown(0.5f, 0.3f, transform));
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
