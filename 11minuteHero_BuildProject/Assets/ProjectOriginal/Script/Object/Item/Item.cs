using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected bool isGet; //ȹ���� ���������� üũ
    protected IEnumerator anim; //�ڷ�ƾ ����
    public void InitItem(Vector3 pos) //������ ���� �� �ʱ�ȭ
    {
        transform.position = pos;
        anim = TweeningUtility.UpAndDown(0.5f, 0.3f, transform); //����ƽ �ڷ�ƾ�� ������ ������ ���� (�ڷ�ƾ �ߴ� ����� ����ϱ� ����)
        gameObject.SetActive(true);

        StartCoroutine(anim);//�������� �յ� ���ִ� ���� �ִϸ��̼� ����
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
        direction.y = 0; //���� ������ y���� 0���� �ؼ� y�� �̵� ����
        float timer = 0;

        while (timer < 0.5f)//�÷��̾� ���� �������� ��ġ�� �������� ƨ��� ���� �ִϸ��̼� ����
        {
            timer += Time.deltaTime;
            transform.position += direction * 4 * Time.deltaTime;
            yield return null;
        }
        while (Vector3.Distance(transform.position, InGameManager.Instance.Player.transform.position + Vector3.up * 0.3f) > 0.2f) //�÷��̾� ��ġ�� �������� ���� �ִϸ��̼� ����
        {
            timer += Time.deltaTime;
            transform.position += (InGameManager.Instance.Player.transform.position + Vector3.up * 0.3f - transform.position).normalized * (8 + timer) * Time.deltaTime;
            yield return null;
        }
        Debug.Log("������ ȹ��");
    }
}
