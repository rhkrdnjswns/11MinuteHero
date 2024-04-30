using UnityEngine;

public class Monster : Character
{
    [SerializeField] private GameObject damageUIPrefab; //������ UI ������
    private DamageUIContainer damageUIContainer = new DamageUIContainer(); //������ UI�� ������ �����̳�

    public void InitDamageUIContainer(float offsetY = 0)
    {
        damageUIContainer.InitDamageUIContainer(transform, 20, damageUIPrefab, offsetY);
    }
    public override void Hit(float damage) //���� �ǰ� �Լ�
    {
        base.Hit(damage);
        damageUIContainer.ActiveDamageUI(damage);
    }
}
