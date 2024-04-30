using System.Collections;
using UnityEngine;

public class WispObject : ActiveObject //������� ������Ʈ
{
#if UNITY_EDITOR
    public int index;
#endif

    public override void IncreaseSize(float value)
    {
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.localScale += Vector3.one * value;
        }
    }
    protected override IEnumerator Co_Activation()
    {
        while (true) //�÷��̾� ������ ȸ���ϴ� �ڷ�ƾ
        {
            transform.RotateAround(InGameManager.Instance.Player.transform.position, Vector3.up, 120 * Time.deltaTime);
            transform.LookAt(InGameManager.Instance.Player.transform);

            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER))
        {
            if (other.TryGetComponent(out Character c))
            {
                c.Hit(damage);
#if UNITY_EDITOR
                InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
                InGameManager.Instance.SkillManager.ActiveSkillList[index].AttackCount++;
#endif
            }
        }
    }
}
