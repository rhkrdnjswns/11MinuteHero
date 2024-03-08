using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{
    private byte energyCount = 5; //검기 방출에 필요한 공격 횟수
    private byte currentEnergyCount; //현재 공격 횟수. 검기 방출 시 0으로 초기화

    private ParticleSystem swordEnergyParticle; //검기 파티클
    private ParticleSystem.MainModule main;

    [SerializeField] private GameObject swordEnergyPrefab; // 검기 프리팹
    private SwordEnergy swordEnergy; // 검기 클래스 레퍼런스

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
        //if (isMaxLevel) InGameManager.Instance.Player.Recovery(inRangeMonsterList.Count); //최대 레벨인 경우 입힌 피해량의 n%만큼 회복
    }
    private void ReleaseEnergy() //검기 방출 함수
    {
        swordEnergyParticle.transform.SetParent(null); //검기 파티클 재생 및 공격 횟수 초기화
        swordEnergyParticle.Play();    

        currentEnergyCount = 0;
    }
    private void CreateSwordEnergy() //검기 프리팹 생성 및 초기화
    {
        GameObject obj = Instantiate(swordEnergyPrefab);

        obj.transform.SetParent(GameObject.Find(ConstDefine.NAME_PROJECTILE_SPAWN_POINT).transform);
        obj.transform.localPosition = Vector3.zero;

        swordEnergyParticle = obj.GetComponent<ParticleSystem>();

        swordEnergy = obj.GetComponent<SwordEnergy>();
        swordEnergy.InitSwordEnergy(damage);
    }
}
