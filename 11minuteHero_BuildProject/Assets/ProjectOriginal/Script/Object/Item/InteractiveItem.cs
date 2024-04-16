using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveItem : Item
{
    protected override IEnumerator Co_ItemAnimation(Vector3 direction)
    {
        if (upAndDownAnim != null)
        {
            StopCoroutine(upAndDownAnim);
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
        Interaction();
    }
}
