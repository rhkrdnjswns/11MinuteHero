using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnergy : MonoBehaviour
{
    private ParticleSystem particleSystem; //�˱� ��ƼŬ
    private SphereCollider collider; //�˱� �ݶ��̴�
    private float damage; //�˱� ������

    private float speed;
    public void SetDamage(float damage) //������ ���� (���� ��ȭ �ø��� ȣ��)
    {
        this.damage = damage;
    }
    public void SetEnergyForMaxLevel()
    {
        speed = 4;
        transform.localScale = Vector3.one * 0.8f;
    }
    public void InitSwordEnergy(float damage) //ó�� �˱� ���� ���� �ʱ�ȭ
    {
        this.damage = damage;
        speed = 1.5f;
        particleSystem = GetComponent<ParticleSystem>();
        collider = GetComponent<SphereCollider>();
        collider.enabled = false;
    }
    private void Start() //�˱� ������Ʈ�� ��Ȱ��ȭ���� �ʱ� ������ Start���� �ڷ�ƾ ȣ���� �� ���� ���ָ� �ȴ�.
    {
        StartCoroutine(Co_MoveEnergy());
    }
    private IEnumerator Co_MoveEnergy()
    {
        Transform origin = GameObject.Find(ConstDefine.NAME_PROJECTILE_SPAWN_POINT).transform;
        while (true)
        {
            yield return new WaitUntil(() => particleSystem.isPlaying); //��ƼŬ�� �÷��� �� ������(�˱� ���� �ø���) �������� �̵�
            collider.enabled = true;
            while (particleSystem.isPlaying)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                yield return InGameManager.Instance.FrameDelay;
            }
            //�˱� ���� ���� �� �˱� ���� �ʱ�ȭ
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
