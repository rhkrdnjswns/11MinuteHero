using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMagicMissile : Projectile //�����簡 ����ϴ� ����ü
{
    private int[] sideArray = {-5, -4, -3, 3, 4, 5 }; //��� ��Ʈ�� ����Ʈ �迭 (������ ����, ����� ������)
    private int side; // ���� ����ü�� �׸� ��� ��Ʈ�� ����Ʈ

    private float timer; //����ü�� ��ǥ���� �����ϴ� �ð�

    private Transform target; //����ü�� Ÿ��(� ������)
    
    private Vector3 summonPos; //����ü�� ������ ��ġ(� ��������)
    private Vector3 controllPoint; //� ��Ʈ�� ����Ʈ

    private Vector3 targetPosByOffset = Vector3.up * 0.5f; //����ü�� ���̿� ���� Ÿ�� ��ġ
  
    public override void ShotProjectile(Transform target) //����ü ���ư��� �Լ� ������. ���� ������ ����
    {
        this.target = target; //Ÿ�� Ʈ������ ����
        side = sideArray[Random.Range(0, sideArray.Length)];
        timer = 0; //�ð� ���� �ʱ�ȭ

        summonPos = InGameManager.Instance.Player.transform.position; //� ��������
        summonPos.y = 0.5f;
        //� ������. Ÿ�������� ���⺤���� ������ side���� ���� �� / �� �����ϰ� ����Ʈ�� ������ ��� �ְ� ��
        controllPoint = InGameManager.Instance.Player.transform.position + Vector3.Cross(target.position - InGameManager.Instance.Player.transform.position, Vector3.up).normalized * side;

        base.ShotProjectile();
    }
    public override void IncreaseSize(float value)
    {
        foreach (var item in GetComponentsInChildren<Transform>())
        {
            item.localScale += Vector3.one * (value / 100f);
        }
    }
    protected override IEnumerator Co_Shot() //����ü ���ư��� �ڷ�ƾ ������.
    {
        while (true)
        {
            timer += Time.deltaTime * 1.8f; //Ÿ�̸� ����. timer = ������ ������ ��Į��. 0 -> 1���� Ŀ��
            targetPosByOffset.x = target.position.x;
            targetPosByOffset.z = target.position.z;

            //������ � �������� �ش� ����Ʈ�� ������ �׷����� ��� timer��ŭ�� ������ ��ȯ��
            transform.position = CalculateBezierPoint(summonPos, controllPoint, targetPosByOffset, timer > 1 ? 1 : timer);

            //Ÿ�� ��ġ�� ���������� ��Ʈ���� ���� ����� ó��
            if (Vector3.Distance(transform.position, targetPosByOffset) <= 0.1) rangedAttackUtility.ReturnProjectile(this);

            //Ÿ���� ����� ����� ó��
            if (!target.gameObject.activeSelf) rangedAttackUtility.ReturnProjectile(this);

            yield return null;
        }
    }
    private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0 + 2 * u * t * p1 + tt * p2;
        //��� ����ü ���� ��ġ y���� 0.5�� ���ִ� ó��
        p.y = 0.5f;

        return p;
    }
}
