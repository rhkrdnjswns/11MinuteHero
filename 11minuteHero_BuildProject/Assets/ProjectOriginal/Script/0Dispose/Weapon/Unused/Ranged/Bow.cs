using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : RangedWeapon
{
    [SerializeField] private int count; //발사할 화살 갯수

    private float arrowAngle;
    //화살 갯수에 따른 화살 각도

    //}
    //protected override void Start()
    //{
    //    base.Start();
    //    SetArrowAngle();
    //}
    protected override void SummonProjectile()
    {
        if (projectileQueue.Count <= 0)//풀에 투사체가 없으면 새로 생성
        {
            CreateNewProjectile(projectileCreateCount);
        }
        if(count <= 1) //한개만 쏘는 경우에는 부모 클래스의 해당 함수 호출 후 리턴
        {
            base.SummonProjectile();
            return;
        }

        float angleY = arrowAngle; //각 화살들의 각도

        for (int i = 0; i < count; i++) //화살 개수만큼 반복
        {
            Projectile p = projectileQueue.Dequeue();

            p.transform.SetParent(spawnPoint); //풀에서 꺼낸 투사체 위치 초기화. 회전각은 몇 번째 화살이냐에 따라 초기화
            p.transform.position = spawnPoint.position; 
            p.transform.localRotation = Quaternion.Euler(90, angleY, 0);

            p.transform.SetParent(activatedProjectileParent); //이후 부모를 바꿔줌. 안바꿔주면 생성위치에 종속되서 플레이어 이동을 투사체가 따라옴

            p.gameObject.SetActive(true);
  
            p.ShotProjectile(); //투사체 발사 함수 호출

            angleY += 10f; //다음 화살의 각도 조정
        }
    }
    private void SetArrowAngle() //발사하는 화살 갯수에 따라 각 화살의 각도를 다르게 설정
    {
        arrowAngle = count > 1 ? -5f + (-(count - 2) * 5f) : 0;
    }
}
