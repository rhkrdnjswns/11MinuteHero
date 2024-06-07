using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorObject : ActiveObject //���� �浹ó���� �� ��ȯ�� ���׿�
{
    [SerializeField] private ParticleSystem particleSystem; //��ƼŬ

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
    protected override IEnumerator Co_Activation() //���׿� �ݶ��̴� Ȱ��ȭ
    {
        particleSystem.Play();
        yield return particleStartDelay; //���׿� ���� ��ƼŬ Ÿ�ֿ̹� ���� �ڷ�ƾ ����
      
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
