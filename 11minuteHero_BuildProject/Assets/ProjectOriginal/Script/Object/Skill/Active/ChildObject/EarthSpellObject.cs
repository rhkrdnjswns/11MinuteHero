using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpellObject : MonoBehaviour
{ 
    [SerializeField] private ParticleSystem particleSystem; //파티클

    private AttackRadiusUtility attackRadiusUtility; //반경 공격기능 참조
    public EarthSpellObject SetAttackRadiusUtility(AttackRadiusUtility reference) //체이닝을 위한 반환타입
    {
        attackRadiusUtility = reference;
        return this;
    }
    public void ActivateSkill(Transform parent, Queue<EarthSpellObject> queue, float damage, float duration, float percentage)  //오브젝트 활성화
    {
        //transform.position = InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f; //위치 조정

        particleSystem.Play();
        StartCoroutine(Co_Activation(parent, queue, damage, duration, percentage));
    }
    private IEnumerator Co_Activation(Transform parent, Queue<EarthSpellObject> queue, float damage, float duration, float percentage)
    {
        yield return new WaitForSeconds(1.6f); //지진 파티클 타이밍에 맞게 반경 내의 몬스터 타격
        attackRadiusUtility.AttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform), damage, EAttackType.Slow, duration, percentage);
        yield return new WaitForSeconds(1.5f); //파티클 종료 딜레이

        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        queue.Enqueue(this);
    }
}
