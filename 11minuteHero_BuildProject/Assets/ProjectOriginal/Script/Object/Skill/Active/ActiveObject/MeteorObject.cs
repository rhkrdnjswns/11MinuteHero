using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorObject : ActiveObject //실제 충돌처리를 할 소환된 메테오
{
    [SerializeField] private ParticleSystem particleSystem; //파티클

    private float radius;

    private readonly WaitForSeconds particleStartDelay = new WaitForSeconds(0.6f);
    private readonly WaitForSeconds particleEndDelay = new WaitForSeconds(3f);

    private Collider[] collisionArray = new Collider[20];
#if UNITY_EDITOR
    public int index;
#endif
    public override void SetAttackRadius(float radius)
    {
        this.radius = radius;
    }
    protected override IEnumerator Co_Activation() //메테오 콜라이더 활성화
    {
        particleSystem.Play();
        yield return particleStartDelay; //메테오 폭발 파티클 타이밍에 맞춰 코루틴 지연
      
        int num = Physics.OverlapSphereNonAlloc(transform.position, radius, collisionArray, ConstDefine.LAYER_MONSTER);
        AttackInRangeUtility.AttackLayerInRange(collisionArray, damage, num);
#if UNITY_EDITOR
        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += num * damage;
#endif
        yield return particleEndDelay;
        owner.ReturnActiveObject(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
