using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpellObject : MonoBehaviour
{ 
    [SerializeField] private ParticleSystem particleSystem; //��ƼŬ

    private AttackRadiusUtility attackRadiusUtility; //�ݰ� ���ݱ�� ����
    public EarthSpellObject SetAttackRadiusUtility(AttackRadiusUtility reference) //ü�̴��� ���� ��ȯŸ��
    {
        attackRadiusUtility = reference;
        return this;
    }
    public void ActivateSkill(Transform parent, Queue<EarthSpellObject> queue, float damage, float duration, float percentage)  //������Ʈ Ȱ��ȭ
    {
        //transform.position = InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f; //��ġ ����

        particleSystem.Play();
        StartCoroutine(Co_Activation(parent, queue, damage, duration, percentage));
    }
    private IEnumerator Co_Activation(Transform parent, Queue<EarthSpellObject> queue, float damage, float duration, float percentage)
    {
        yield return new WaitForSeconds(1.6f); //���� ��ƼŬ Ÿ�ֿ̹� �°� �ݰ� ���� ���� Ÿ��
        attackRadiusUtility.AttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform), damage, EAttackType.Slow, duration, percentage);
        yield return new WaitForSeconds(1.5f); //��ƼŬ ���� ������

        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        queue.Enqueue(this);
    }
}
