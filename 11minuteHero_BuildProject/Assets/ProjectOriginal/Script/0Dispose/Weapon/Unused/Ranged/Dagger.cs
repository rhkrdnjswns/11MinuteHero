using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : RangedWeapon
{
    private List<DaggerProjectile> daggerProjectileList = new List<DaggerProjectile>();
    protected override void SetProjectile() //투사체 초기화 시 투사체 단검들을 리스트에 초기화
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
        //무기 최대 레벨에 도달한 경우 아래 로직 실행

        float angleY = 0; //각 단검들의 각도

        for (int i = 0; i < 9; i++) //단검 총 9개 사출
        {
            Projectile p = projectileQueue.Dequeue();

            p.transform.SetParent(spawnPoint); //풀에서 꺼낸 투사체 위치 초기화. 회전각은 몇 번째 화살이냐에 따라 초기화
            p.transform.position = spawnPoint.position;
            p.transform.localRotation = Quaternion.Euler(90, angleY, 0);

            p.transform.SetParent(activatedProjectileParent); //이후 부모를 바꿔줌. 안바꿔주면 생성위치에 종속되서 플레이어 이동을 투사체가 따라옴

            p.gameObject.SetActive(true);

            p.ShotProjectile(); //투사체 발사 함수 호출

            angleY += 45f; //다음 단검의 각도 조정
        }
    }
}
