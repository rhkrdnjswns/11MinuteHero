using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerProjectile : Projectile
{
    [SerializeField] private float rotationSpeed = 80; // �ܰ� ȸ�� �ӵ�. ��ġ�� ���� ���� �� ���� ���� �׸���.
    private int count = 1; //���� ������ Ƚ��
    private byte currentCount; //���� ������ Ƚ��

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
            //other.GetComponent<Monster>().Hit(damage); //Monster Ŭ������ �����Ͽ� ������ ����
            if (IsMaxLevel) return;
            if(++currentCount >= count)
            {
                currentCount = 0;
                rangedAttackUtility.ReturnProjectile(this); //���� Ǯ�� �ǵ���
            }
        }
    }
    private IEnumerator Co_DaggerTyphoon() //�ܰ��� �÷��̾ �߽����� ���� �׸��� ���ư�
    {
        float speed = 0;
        while (true)
        {
           // if (Vector3.Distance(transform.position, summonPos) > 5) rangedSkillUtility.ReturnProjectile(this); //������ ��ġ���� ��Ÿ���ŭ ���ư� ��� Ǯ�� �ǵ���
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            transform.position += transform.up * speed * Time.deltaTime;
            //speed += Time.deltaTime * base.speed;
            yield return InGameManager.Instance.FrameDelay;
        }
    }
    private IEnumerator Co_DaggerParade() //�ܰ��� ������ ��ǥ�� n�� �����Ͽ� ���ư�
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
