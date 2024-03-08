using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : RangedWeapon
{
    private List<DaggerProjectile> daggerProjectileList = new List<DaggerProjectile>();
    protected override void SetProjectile() //����ü �ʱ�ȭ �� ����ü �ܰ˵��� ����Ʈ�� �ʱ�ȭ
    {
        base.SetProjectile();
        foreach (var daggerProjectile in projectileQueue)
        {
            daggerProjectileList.Add(daggerProjectile.GetComponent<DaggerProjectile>());
        }
    }

    protected override void SummonProjectile()
    {
        if (true)
        {
            base.SummonProjectile();
            return;
        }
        //���� �ִ� ������ ������ ��� �Ʒ� ���� ����

        float angleY = 0; //�� �ܰ˵��� ����

        for (int i = 0; i < 9; i++) //�ܰ� �� 9�� ����
        {
            Projectile p = projectileQueue.Dequeue();

            p.transform.SetParent(spawnPoint); //Ǯ���� ���� ����ü ��ġ �ʱ�ȭ. ȸ������ �� ��° ȭ���̳Ŀ� ���� �ʱ�ȭ
            p.transform.position = spawnPoint.position;
            p.transform.localRotation = Quaternion.Euler(90, angleY, 0);

            p.transform.SetParent(activatedProjectileParent); //���� �θ� �ٲ���. �ȹٲ��ָ� ������ġ�� ���ӵǼ� �÷��̾� �̵��� ����ü�� �����

            p.gameObject.SetActive(true);

            p.ShotProjectile(); //����ü �߻� �Լ� ȣ��

            angleY += 45f; //���� �ܰ��� ���� ����
        }
    }
}
