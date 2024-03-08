using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : RangedWeapon
{
    private byte count = 5; //폭발 공격에 필요한 공격 횟수
    private byte currentCount;


    protected override void SummonProjectile()
    {
        if (projectileQueue.Count <= 0)//풀에 투사체가 없으면 새로 생성
        {
            CreateNewProjectile(projectileCreateCount);
        }

        Projectile p = projectileQueue.Dequeue();

        p.transform.SetParent(spawnPoint); //풀에서 꺼낸 투사체 위치, 회전각 생성위치에 맞게 초기화
        p.transform.position = spawnPoint.position;
        p.transform.localRotation = Quaternion.Euler(-90, 0, 0); //파이어볼의 x축 회전각은 -90임

        p.transform.SetParent(activatedProjectileParent); //이후 부모를 바꿔줌. 안바꿔주면 생성위치에 종속되서 플레이어 이동을 투사체가 따라옴
        p.gameObject.SetActive(true);

        currentCount++;

       // p.ShotProjectile(spawnPoint.position, currentCount >= count); //오버로딩한 함수 호출. 횟수 충족 시 true

        if (currentCount >= count) //횟수 초기화
        {
            currentCount = 0;
        }
    }
}
