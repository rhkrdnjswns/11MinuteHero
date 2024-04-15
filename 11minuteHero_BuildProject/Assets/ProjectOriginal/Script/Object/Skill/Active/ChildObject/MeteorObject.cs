using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorObject : MonoBehaviour //���� �浹ó���� �� ��ȯ�� ���׿�
{
    private ParticleSystem particleSystem; //��ƼŬ

    private AttackRadiusUtility attackRadiusUtility;
#if UNITY_EDITOR
    public int index;
#endif
    private void Awake() //���� ������
    {
        particleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    public MeteorObject SetAttackRadiusUtility(AttackRadiusUtility reference)
    {
        attackRadiusUtility = reference;
        return this;
    }
    public void ActivateSkill(Transform parent, Queue<MeteorObject> queue, float damage) //���׿� ��ƼŬ ���
    {
        particleSystem.Play();
        StartCoroutine(Co_Activation(parent, queue, damage));
    }
    private IEnumerator Co_Activation(Transform parent, Queue<MeteorObject> queue, float damage) //���׿� �ݶ��̴� Ȱ��ȭ
    {
        yield return new WaitForSeconds(0.6f); //���׿� ���� ��ƼŬ Ÿ�ֿ̹� ���� �ڷ�ƾ ����
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
