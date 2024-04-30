using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePenetration : Projectile
{
    protected int count; //관통 가능한 횟수
    protected int currentCount; //현재 관통한 횟수
    protected float activateTime; //활성 시간
    public override void SetActivateTime(float time)
    {
        activateTime = time;
    }
    public override void SetPenetrationCount(int count)
    {
        this.count = count;
        currentCount = count;
    }
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        while (timer < activateTime)
        {
            transform.position += shotDirection * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        currentCount = count;
        owner.ReturnProjectile(this);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            other.GetComponent<Character>().Hit(damage); //Monster 클래스를 추출하여 데미지 연산
            currentCount--;
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
            if (currentCount > 0) return;
            currentCount = count; //관통 횟수를 전부 소모한 경우 투사체 회수
            owner.ReturnProjectile(this);
        }
    }
}
