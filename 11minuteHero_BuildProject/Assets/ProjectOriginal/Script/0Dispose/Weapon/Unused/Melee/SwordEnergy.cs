using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnergy : MonoBehaviour
{
    private ParticleSystem particleSystem; //검기 파티클
    private SphereCollider collider; //검기 콜라이더
    private float damage; //검기 데미지

    private float speed;
    public void SetDamage(float damage) //데미지 설정 (무기 강화 시마다 호출)
    {
        this.damage = damage;
    }
    public void SetEnergyForMaxLevel()
    {
        speed = 4;
        transform.localScale = Vector3.one * 0.8f;
    }
    public void InitSwordEnergy(float damage) //처음 검기 생성 시의 초기화
    {
        this.damage = damage;
        speed = 1.5f;
        particleSystem = GetComponent<ParticleSystem>();
        collider = GetComponent<SphereCollider>();
        collider.enabled = false;
    }
    private void Start() //검기 오브젝트는 비활성화하지 않기 때문에 Start에서 코루틴 호출을 한 번만 해주면 된다.
    {
        StartCoroutine(Co_MoveEnergy());
    }
    private IEnumerator Co_MoveEnergy()
    {
        Transform origin = GameObject.Find(ConstDefine.NAME_PROJECTILE_SPAWN_POINT).transform;
        while (true)
        {
            yield return new WaitUntil(() => particleSystem.isPlaying); //파티클이 플레이 될 때마다(검기 방출 시마다) 전방으로 이동
            collider.enabled = true;
            while (particleSystem.isPlaying)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                yield return InGameManager.Instance.FrameDelay;
            }
            //검기 방출 종료 후 검기 설정 초기화
            collider.enabled = false;
            transform.SetParent(origin);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER))
        {
            other.GetComponent<Monster>().Hit(damage);
        }
    }
}
