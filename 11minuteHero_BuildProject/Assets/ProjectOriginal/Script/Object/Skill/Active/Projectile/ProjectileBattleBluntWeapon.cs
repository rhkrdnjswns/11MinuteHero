using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBattleBluntWeapon : Projectile
{
    private float distance; //투사체가 날아가는 거리
    private float rotateSpeed; //자전 속도
    private Vector3 summonPos; //투사체가 날아간 거리 체크를 위한 생성 포인트
    private float knockBackSpeed;
    private float knockBackDuration;

    [SerializeField] private Transform objTransform; //투사체 메쉬 오브젝트의 트랜스폼 (회전 효과를 위한 참조)

    private System.Action IncreaseReturnCount;

    private bool isReturn; //투사체가 다시 돌아오는 경우는 넉백기능 X

    public override void SetKnockBackData(float speed, float duration)
    {
        knockBackSpeed = speed;
        knockBackDuration = duration;
    }
    public override void SetRotationSpeed(float speed)
    {
        rotateSpeed = speed;
    }
    public override void SetAction(System.Action action)
    {
        IncreaseReturnCount += action;
    }
    public override void SetDistance(float distance)
    {
        this.distance = distance;
    }
    public override void ShotProjectile() //투사체 생성 위치와 날아갈 방향 갱신하고 날아가는 코루틴 실행
    {
        summonPos = InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f;
        isReturn = false;

        base.ShotProjectile();
        StartCoroutine(Co_Rotate());
    }
    protected override IEnumerator Co_Shot()
    {
        while (Vector3.Distance(summonPos, transform.position) < distance) //소환 후 사거리만큼 날아감
        {
            transform.position += shotDirection.normalized * speed * Time.deltaTime;
            yield return null;
        }
        isReturn = true;
        while (Vector3.Distance(InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f, transform.position) > 0.5f) //이후 플레이어 위치로 돌아감
        {
            Vector3 direction = InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f - transform.position;//플레이어가 계속 이동할 수 있기 때문에 위치를 프레임마다 갱신
            transform.position += direction.normalized * speed * Time.deltaTime;
            yield return null;
        }
        IncreaseReturnCount(); //회수 카운트 증가시킴

        owner.ReturnProjectile(this); //투사체 회수
    }
    private IEnumerator Co_Rotate() //전투망치가 활성화 된 동안에는 계속 회전하도록 하는 코루틴
    {
        while (true)
        {
            objTransform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
    protected override void OnTriggerEnter(Collider other) //투사체 충돌 처리
    {
        if (other.CompareTag(targetTag)) //몬스터와 부딪힌 경우
        {
            Character c = other.GetComponent<Character>();
            c.Hit(damage); //Monster 클래스를 추출하여 데미지 연산
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
            if (!c.IsDie && !isReturn) c.KnockBack(knockBackSpeed, knockBackDuration, (c.transform.position - transform.position).normalized);
        }
    }
}
