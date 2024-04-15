using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispObject : MonoBehaviour //������ �浹 ������ �� ������� Ŭ����
{
    private float damage;
    private Vector3 prevPosition;
#if UNITY_EDITOR
    public int index;
#endif
    public void SetWsip(float damage) //�������� �������ְ� ȸ�� �ڷ�ƾ ����
    {
        this.damage = damage;
        StartCoroutine(Co_Rotate());
        prevPosition = transform.localPosition;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    private IEnumerator Co_Rotate()
    {
        while (true) //�÷��̾� ������ ȸ���ϴ� �ڷ�ƾ
        {
            transform.RotateAround(InGameManager.Instance.Player.transform.position, Vector3.up, 120 * Time.deltaTime);
            transform.LookAt(InGameManager.Instance.Player.transform);

            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(ConstDefine.TAG_MONSTER)) //���� �浹 �� damage��ŭ ���� ����
        {
            other.GetComponent<Character>().Hit(damage);
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
            InGameManager.Instance.SkillManager.ActiveSkillList[index].AttackCount++;
#endif
        }
    }
    //private void Update() //�����Ӹ��� ���� ��ġ�� ���� ��ġ���� ���� ���͸� ���� forwar�� ��������. ������Ʈ�� ���� �˵��� ��� �ٶ󺸵��� �ϴ� ó��
    //{
    //    //Vector3 rotationDirection = (prevPosition - transform.localPosition).normalized; 
    //    //transform.forward = rotationDirection;

    //    //prevPosition = transform.localPosition;
    //} -> �����غ��� �׳� ȭ����ǳ ������Ʈ�� ���� ��ƼŬ ������ ����̸� �Ǵ� ��������. �ڵ�� ó���� �ʿ䰡 ����

}
