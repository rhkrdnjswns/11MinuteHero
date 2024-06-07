using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpellObject : ActiveObject
{ 
    [SerializeField] private ParticleSystem particleSystem; //파티클
    private float radius;

    private readonly WaitForSeconds particleStartDelay = new WaitForSeconds(1.6f);
    private readonly WaitForSeconds particleEndDelay = new WaitForSeconds(1.5f);

    private Collider[] collisionArray = new Collider[60];
    private float slowDownValue;
    private float slowDownDuration;

#if UNITY_EDITOR
    public int index;
#endif
    public override void SetAttackRadius(float radius)
    {
        this.radius = radius;
    }
    public override void SetSlowDownData(float value, float duration)
    {
        slowDownValue = value;
        slowDownDuration = duration;
    }
    public override void IncreaseSize(float value)
    {
        transform.localScale += (Vector3.one - Vector3.up) * value;
    }
    protected override IEnumerator Co_Activation()
    {
        particleSystem.Play();
        yield return particleStartDelay; //지진 파티클 타이밍에 맞게 반경 내의 몬스터 타격
        int num = Physics.OverlapSphereNonAlloc(transform.position, radius, collisionArray, ConstDefine.LAYER_MONSTER);
        AttackInRangeUtility.AttackAndSlowDownLayerInRange(collisionArray, damage, num, slowDownValue, slowDownDuration);
#if UNITY_EDITOR
        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += num * damage;
#endif
        yield return particleEndDelay; //파티클 종료 딜레이

        owner.ReturnActiveObject(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
