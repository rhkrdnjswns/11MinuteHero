using UnityEngine;

public class Monster : Character
{
    [SerializeField] private GameObject damageUIPrefab; //데미지 UI 프리팹
    protected DamageUIContainer damageUIContainer = new DamageUIContainer(); //데미지 UI를 관리할 컨테이너

    public virtual void InitMonsterData()
    {

    }
    public void InitDamageUIContainer(float offsetY = 0)
    {
        damageUIContainer.InitDamageUIContainer(transform, 20, damageUIPrefab, offsetY);
    }
    public override void Hit(float damage) //몬스터 피격 함수
    {
        if (IsDie) return;
        base.Hit(damage);
        damageUIContainer.ActiveDamageUI(damage);
    }
}
