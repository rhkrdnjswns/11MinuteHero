using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerProjectile : Projectile
{
    [SerializeField] private float rotationSpeed = 80; // 단검 회전 속도. 수치가 높을 수록 더 많은 원을 그린다.
    private int count = 1; //관통 가능한 횟수
    private byte currentCount; //현재 관통한 횟수

    private byte starCount;
    public void IncreaseCount()
    {
        count++;
    }
    //public override void ShotProjectile(Vector3 pos)
    //{
    //    if (IsMaxLevel)
    //    {
    //        originPos = pos;
    //        shotDirection = transform.up;
    //        //StartCoroutine(Co_DaggerPeraid());
    //        return;
    //    }
    //    base.ShotProjectile(pos);
    //}
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER))
        {
            //other.GetComponent<Monster>().Hit(damage); //Monster 클래스를 추출하여 데미지 연산
            if (IsMaxLevel) return;
            if(++currentCount >= count)
            {
                currentCount = 0;
                rangedAttackUtility.ReturnProjectile(this); //이후 풀로 되돌림
            }
        }
    }
    private IEnumerator Co_DaggerTyphoon() //단검이 플레이어를 중심으로 원을 그리며 날아감
    {
        float speed = 0;
        while (true)
        {
           // if (Vector3.Distance(transform.position, summonPos) > 5) rangedSkillUtility.ReturnProjectile(this); //생성된 위치에서 사거리만큼 날아간 경우 풀로 되돌림
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            transform.position += transform.up * speed * Time.deltaTime;
            //speed += Time.deltaTime * base.speed;
            yield return InGameManager.Instance.FrameDelay;
        }
    }
    private IEnumerator Co_DaggerParade() //단검이 랜덤한 좌표를 n번 생성하여 날아감
    {
        while (true)
        {
            //while(Vector3.Distance(transform.position, summonPos) <= 5)
            //{
            //    transform.position += transform.up * speed * Time.deltaTime;
            //    yield return InGameManager.Instance.FrameDelay;
            //}
            //if (starCount > 4)
            //{
            //    starCount = 0;
            //    rangedSkillUtility.ReturnProjectile(this);
            //}
            //summonPos = transform.position;
            //transform.Rotate(Vector3.forward * Random.Range(0, 360));
            //starCount++;
            //yield return InGameManager.Instance.FrameDelay;
        }
    }

    protected override IEnumerator Co_Shot()
    {
        throw new System.NotImplementedException();
    }
}
