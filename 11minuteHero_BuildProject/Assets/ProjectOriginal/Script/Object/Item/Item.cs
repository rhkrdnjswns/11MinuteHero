using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected bool isGet; //획득한 아이템인지 체크
    protected IEnumerator anim; //코루틴 참조
    public void InitItem(Vector3 pos) //아이템 생성 시 초기화
    {
        transform.position = pos;
        anim = TweeningUtility.UpAndDown(0.5f, 0.3f, transform); //스태틱 코루틴을 열거자 변수로 참조 (코루틴 중단 기능을 사용하기 위해)
        gameObject.SetActive(true);

        StartCoroutine(anim);//아이템이 둥둥 떠있는 듯한 애니메이션 적용
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
    private IEnumerator Co_ItemAnimation(Vector3 direction)
    {
        if(anim != null)
        {
            StopCoroutine(anim);
        }
        transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
        direction.y = 0; //방향 벡터의 y값을 0으로 해서 y축 이동 제한
        float timer = 0;

        while (timer < 0.5f)//플레이어 기준 아이템이 위치한 방향으로 튕기는 듯한 애니메이션 적용
        {
            timer += Time.deltaTime;
            transform.position += direction * 4 * Time.deltaTime;
            yield return null;
        }
        while (Vector3.Distance(transform.position, InGameManager.Instance.Player.transform.position + Vector3.up * 0.3f) > 0.2f) //플레이어 위치로 빨려가는 듯한 애니메이션 적용
        {
            timer += Time.deltaTime;
            transform.position += (InGameManager.Instance.Player.transform.position + Vector3.up * 0.3f - transform.position).normalized * (8 + timer) * Time.deltaTime;
            yield return null;
        }
        Debug.Log("아이템 획득");
    }
}
