using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : RangedWeapon
{
    [SerializeField] private int count; //�߻��� ȭ�� ����

    private float arrowAngle;
    //ȭ�� ������ ���� ȭ�� ����

    //}
    //protected override void Start()
    //{
    //    base.Start();
    //    SetArrowAngle();
    //}
    protected override void SummonProjectile()
    {
        if (projectileQueue.Count <= 0)//Ǯ�� ����ü�� ������ ���� ����
        {
            CreateNewProjectile(projectileCreateCount);
        }
        if(count <= 1) //�Ѱ��� ��� ��쿡�� �θ� Ŭ������ �ش� �Լ� ȣ�� �� ����
        {
            base.SummonProjectile();
            return;
        }

        float angleY = arrowAngle; //�� ȭ����� ����

        for (int i = 0; i < count; i++) //ȭ�� ������ŭ �ݺ�
        {
            Projectile p = projectileQueue.Dequeue();

            p.transform.SetParent(spawnPoint); //Ǯ���� ���� ����ü ��ġ �ʱ�ȭ. ȸ������ �� ��° ȭ���̳Ŀ� ���� �ʱ�ȭ
            p.transform.position = spawnPoint.position; 
            p.transform.localRotation = Quaternion.Euler(90, angleY, 0);

            p.transform.SetParent(activatedProjectileParent); //���� �θ� �ٲ���. �ȹٲ��ָ� ������ġ�� ���ӵǼ� �÷��̾� �̵��� ����ü�� �����

            p.gameObject.SetActive(true);
  
            p.ShotProjectile(); //����ü �߻� �Լ� ȣ��

            angleY += 10f; //���� ȭ���� ���� ����
        }
    }
    private void SetArrowAngle() //�߻��ϴ� ȭ�� ������ ���� �� ȭ���� ������ �ٸ��� ����
    {
        arrowAngle = count > 1 ? -5f + (-(count - 2) * 5f) : 0;
    }
}
