using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispObject : MonoBehaviour //실제로 충돌 연산을 할 도깨비불 클래스
{
    private float damage;
    private Vector3 prevPosition;
#if UNITY_EDITOR
    public int index;
#endif
    public void SetWsip(float damage) //데미지를 설정해주고 회전 코루틴 실행
    {
        this.damage = damage;
        StartCoroutine(Co_Rotate());
        prevPosition = transform.localPosition;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    private IEnumerator Co_Rotate()
    {
        while (true) //플레이어 주위를 회전하는 코루틴
        {
            transform.RotateAround(InGameManager.Instance.Player.transform.position, Vector3.up, 120 * Time.deltaTime);
            transform.LookAt(InGameManager.Instance.Player.transform);

            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(ConstDefine.TAG_MONSTER)) //몬스터 충돌 시 damage만큼 피해 입힘
        {
            other.GetComponent<Character>().Hit(damage);
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
            InGameManager.Instance.SkillManager.ActiveSkillList[index].AttackCount++;
#endif
        }
    }
    //private void Update() //프레임마다 이전 위치와 현재 위치로의 방향 벡터를 구해 forwar로 설정해줌. 오브젝트가 공전 궤도를 계속 바라보도록 하는 처리
    //{
    //    //Vector3 rotationDirection = (prevPosition - transform.localPosition).normalized; 
    //    //transform.forward = rotationDirection;

    //    //prevPosition = transform.localPosition;
    //} -> 생각해보니 그냥 화염폭풍 오브젝트의 하위 파티클 각도를 기울이면 되는 문제였다. 코드로 처리할 필요가 없음

}
