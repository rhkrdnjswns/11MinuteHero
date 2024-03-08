using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1 : Skill //��� ���� Ŭ�������� �θ� Ŭ����. (09/25)��Ƽ�� ��ų ���� �Ϸ� �� �����丵 �ÿ� Weapon���� Ŭ���� �̸� �ٲ����
{
    public enum eWeaponType //���� Ÿ��
    {
        Range,
        Melee
    }
    [SerializeField] private eWeaponType weaponType; //���⿡ ���� �ִϸ��̼� ����� ���� ���� Ÿ�� ������


    private ParticleSystem maxLevelParticle; //���� �ִ� ���� ���� �� �÷��̵� ��ƼŬ
    [SerializeField] private GameObject maxLevelParticlePrefab; //���� ��ȭ�� �ִ�ġ�� �������� �� ����� ��ƼŬ

    [SerializeField] protected float coolTime; //���� ��Ÿ��
    private float originCoolTime; //���� �⺻ ��Ÿ��
    [SerializeField] protected float increaseDamage; //���ݷ� ����ġ
    [SerializeField] protected float damage; //���ݷ�

    public eWeaponType WeaponType { get => weaponType; }

    public float CoolTime { get => coolTime; }


    public virtual void InitWeapon() //�÷��̾��� Start���� ȣ���. ���� ���� ������ �ʱ�ȭ�� �ش� �Լ����� ����
    {
        CreateMaxLevelParticle();
        originCoolTime = coolTime;
        level = 1;
        UpdateDamage();
    }
    public virtual void UpdateDamage() //���� ������ ����. �÷��̾� ���ݷ� * (���� ���ݷ� ����ġ * ���� ����)
    {
        damage = increaseDamage * level;
    }
    public virtual void UpdateRange(float percentage) //���� ������ �°� ��Ÿ� ����
    {
        //Do Something
    }
    public virtual void Attack() //���� �Լ� (�ڽİ�ü���� ���� ������)
    {
        return;
    }
    public void ReduceCoolTime(float percentage) //���� ��Ÿ�� ���� �Լ�
    {
        coolTime -= originCoolTime * percentage / 100;
    }
    private void CreateMaxLevelParticle() //���� �ִ뷹�� ��ƼŬ ����
    {
        GameObject obj = Instantiate(maxLevelParticlePrefab);

        obj.transform.SetParent(InGameManager.Instance.Player.transform);
        maxLevelParticle = obj.GetComponent<ParticleSystem>();
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
