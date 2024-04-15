using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorObject : MonoBehaviour //실제 충돌처리를 할 소환된 메테오
{
    private ParticleSystem particleSystem; //파티클

    private AttackRadiusUtility attackRadiusUtility;
#if UNITY_EDITOR
    public int index;
#endif
    private void Awake() //참조 가져옴
    {
        particleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    public MeteorObject SetAttackRadiusUtility(AttackRadiusUtility reference)
    {
        attackRadiusUtility = reference;
        return this;
    }
    public void ActivateSkill(Transform parent, Queue<MeteorObject> queue, float damage) //메테오 파티클 재생
    {
        particleSystem.Play();
        StartCoroutine(Co_Activation(parent, queue, damage));
    }
    private IEnumerator Co_Activation(Transform parent, Queue<MeteorObject> queue, float damage) //메테오 콜라이더 활성화
    {
        yield return new WaitForSeconds(0.6f); //메테오 폭발 파티클 타이밍에 맞춰 코루틴 지연
        attackRadiusUtility.AttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform), damage);
#if UNITY_EDITOR
        int count = attackRadiusUtility.GetLayerInRadius(transform).Length;
        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += count * damage;
#endif
        yield return new WaitForSeconds(3f);
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        queue.Enqueue(this);
    }

}
