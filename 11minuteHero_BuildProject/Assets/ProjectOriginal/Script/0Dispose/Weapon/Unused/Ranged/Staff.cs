using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : RangedWeapon
{
    private byte count = 5; //���� ���ݿ� �ʿ��� ���� Ƚ��
    private byte currentCount;


    protected override void SummonProjectile()
    {
        if (projectileQueue.Count <= 0)//Ǯ�� ����ü�� ������ ���� ����
        {
            CreateNewProjectile(projectileCreateCount);
        }

        Projectile p = projectileQueue.Dequeue();

        p.transform.SetParent(spawnPoint); //Ǯ���� ���� ����ü ��ġ, ȸ���� ������ġ�� �°� �ʱ�ȭ
        p.transform.position = spawnPoint.position;
        p.transform.localRotation = Quaternion.Euler(-90, 0, 0); //���̾�� x�� ȸ������ -90��

        p.transform.SetParent(activatedProjectileParent); //���� �θ� �ٲ���. �ȹٲ��ָ� ������ġ�� ���ӵǼ� �÷��̾� �̵��� ����ü�� �����
        p.gameObject.SetActive(true);

        currentCount++;

       // p.ShotProjectile(spawnPoint.position, currentCount >= count); //�����ε��� �Լ� ȣ��. Ƚ�� ���� �� true

        if (currentCount >= count) //Ƚ�� �ʱ�ȭ
        {
            currentCount = 0;
        }
    }
}
