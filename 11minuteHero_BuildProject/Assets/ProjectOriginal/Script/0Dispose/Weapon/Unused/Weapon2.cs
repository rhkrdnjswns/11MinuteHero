using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon2 : Skill
{
    public enum eWeaponType //���� Ÿ��
    {
        Range,
        Melee
    }
    [SerializeField] private eWeaponType weaponType;

    private ParticleSystem maxLevelParticle;
    [SerializeField] private GameObject maxLevelParticlePrefab; //���� ��ȭ�� �ִ�ġ�� �������� �� ����� ��ƼŬ

    [SerializeField] protected float weaponCoolTime; //���� ��Ÿ��
    [SerializeField] protected float increaseDamage; //���ݷ� ����ġ
    [SerializeField] protected float damage; //���ݷ�

    public virtual void UpdateDamage()
    {
        damage = increaseDamage * level;
    }
    public void ReduceCoolTime(float percentage) //���� ��Ÿ�� ���� �Լ�
    {
        weaponCoolTime -= weaponCoolTime * percentage / 100;
    }
    private void CreateMaxLevelParticle() //���� �ִ뷹�� ��ƼŬ ����
    {
        GameObject obj = Instantiate(maxLevelParticlePrefab);

        obj.transform.SetParent(InGameManager.Instance.Player.transform);
        maxLevelParticle = obj.GetComponent<ParticleSystem>();
    }
    public float WeaponCoolTime { get => weaponCoolTime; }
    public eWeaponType WeaponType { get => weaponType; }

    public virtual void Attack() //���� �Լ� (�ڽİ�ü���� ���� ������)
    {
        return;
    }

    protected override void UpdateSkillData()
    {
        throw new System.NotImplementedException();
    }

    public override void SetEvlotionCondition()
    {
        throw new System.NotImplementedException();
    }
}
