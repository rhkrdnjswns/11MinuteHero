using UnityEngine;

public class Monster : Character
{
    [SerializeField] private GameObject damageUIPrefab; //������ UI ������
    protected DamageUIContainer damageUIContainer = new DamageUIContainer(); //������ UI�� ������ �����̳�

    public virtual void InitMonsterData()
    {

    }
    public void InitDamageUIContainer(float offsetY = 0)
    {
        damageUIContainer.InitDamageUIContainer(transform, 20, damageUIPrefab, offsetY);
    }
    public override void Hit(float damage) //���� �ǰ� �Լ�
    {
        if (IsDie) return;
        base.Hit(damage);
        damageUIContainer.ActiveDamageUI(damage);
    }
}
