using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{
    private byte energyCount = 5; //�˱� ���⿡ �ʿ��� ���� Ƚ��
    private byte currentEnergyCount; //���� ���� Ƚ��. �˱� ���� �� 0���� �ʱ�ȭ

    private ParticleSystem swordEnergyParticle; //�˱� ��ƼŬ
    private ParticleSystem.MainModule main;

    [SerializeField] private GameObject swordEnergyPrefab; // �˱� ������
    private SwordEnergy swordEnergy; // �˱� Ŭ���� ���۷���

    private float originSize = 3;
    //protected override void Start()
    //{
    //    base.Start();
    //    CreateSwordEnergy();
    //    main = particleSystem.main;
    //}

    public override void Attack()
    {
        base.Attack();
        //if (isMaxLevel) InGameManager.Instance.Player.Recovery(inRangeMonsterList.Count); //�ִ� ������ ��� ���� ���ط��� n%��ŭ ȸ��
    }
    private void ReleaseEnergy() //�˱� ���� �Լ�
    {
        swordEnergyParticle.transform.SetParent(null); //�˱� ��ƼŬ ��� �� ���� Ƚ�� �ʱ�ȭ
        swordEnergyParticle.Play();    

        currentEnergyCount = 0;
    }
    private void CreateSwordEnergy() //�˱� ������ ���� �� �ʱ�ȭ
    {
        GameObject obj = Instantiate(swordEnergyPrefab);

        obj.transform.SetParent(GameObject.Find(ConstDefine.NAME_PROJECTILE_SPAWN_POINT).transform);
        obj.transform.localPosition = Vector3.zero;

        swordEnergyParticle = obj.GetComponent<ParticleSystem>();

        swordEnergy = obj.GetComponent<SwordEnergy>();
        swordEnergy.InitSwordEnergy(damage);
    }
}
