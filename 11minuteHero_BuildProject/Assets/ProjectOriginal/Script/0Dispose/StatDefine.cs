using UnityEngine;

[System.Serializable]
public class CommonStats //��� �÷��̾�� ��ȣ�ۿ� ��ü�� ���� �ɷ�ġ Ŭ����
{
    [SerializeField] protected float hp;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float speed;
    [SerializeField] protected float currentSpeed; //���� �ӵ��� ����Ǵ� ��ġ. speed�� �ʱ� ���� ��ġ
    [SerializeField] protected float damage; //�÷��̾��� �������� ����� ��Ϳ� �÷����� �ۿ���

    public CommonStats(float maxHp, float speed, float damage)
    {
        this.maxHp = maxHp;
        hp = maxHp;
        this.speed = speed;
        currentSpeed = speed;
        this.damage = damage;
    }

    //���� ��ġ�� ���� �Լ��� ���� ������ �ʰ� ������Ƽ�� ����
    public float Speed { get => speed; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public float Damage { get => damage; }
    public float MaxHp { get => maxHp; }
    public float Hp { get => hp; }

    public virtual void DecreaseHp(float value) //hp ���� �Լ�. value��ŭ ���ҽ�Ŵ. ���� ü�¹� ����
    {
        hp -= value;
        if (hp < 0) hp = 0;
    }
}
[System.Serializable]
public class MonsterStats : CommonStats //������ �ɷ�ġ Ŭ����. ���� �ɷ�ġ Ŭ���� ���
{
    private int rewardExp; //���� óġ �� �÷��̾� ����ġ ��·�
    public MonsterStats(float maxHp, float speed, float damage, int rewardExp) : base(maxHp, speed, damage) //������ ������
    {
        this.rewardExp = rewardExp;
    }
    public int GetRewardExp()
    {
        return rewardExp;
    }
    public void ResetStats() //���� ������ �ʱ�ȭ
    {
        hp = maxHp;
    }
    public override void DecreaseHp(float value)
    {
        base.DecreaseHp(value);
    }
    public bool IsDie() //���� ��� üũ
    {
        return hp <= 0 ? true : false;
    }
}
[System.Serializable]
public class PlayerStats : CommonStats //�÷��̾��� �ɷ�ġ Ŭ����. ���� �ɷ�ġ Ŭ���� ���
{
    [SerializeField] private int exp;
    [SerializeField] private int maxExp;
    [SerializeField] private int level;

    private float currentDamage; //�������� ��ġ�� ������� �����ϴ� �ɷ�ġ�� ��� �������� current�� ��
    private float currentMaxHp;

    //�нú� ��Ϳ� �����ϴ� �÷��̾� �ɷ�ġ��
    private int expGainPercentage; //����ġ ȹ�淮
    private float reduceDamagePercentage; //���� ���ҷ�


    //��ġ�� ������ �ִ� ������ ���� ������Ƽ�� ���� ��ġ ����. (���� �Լ� �ȸ���)
    public float CurrentDamage { get => currentDamage; set => currentDamage = value; }
    public float CurrentMaxHp { get => currentMaxHp; set => currentMaxHp = value; }
    public int ExpGainPercentage { get => expGainPercentage; set => expGainPercentage = value; }
    public float ReduceDamagePercentage { get => reduceDamagePercentage; set => reduceDamagePercentage = value; }
    public int Level { get => level; }

    public PlayerStats(float maxHp, float speed, float damage, int maxExp) : base(maxHp, speed, damage)
    {
        //�÷��̾� ���� �ʱ�ȭ
        currentDamage = damage;
        currentMaxHp = maxHp;
        this.maxExp = maxExp;
        level = 1;
    }
    public override void DecreaseHp(float value) //ü�� ���� �Լ� ������. �÷��̾�� �ǰ� �� ���� ȿ���� �ַ��� ����������.
    {
        hp -= value - (value * reduceDamagePercentage / 100); //���� ���ҷ��� �����Ͽ� ����
        if (hp < 0) hp = 0;
        if (hp <= 0) InGameManager.Instance.GameState = EGameState.GameOver; //�÷��̾� hp�� 0�� �Ǹ� GameOver 
    }
    public override string ToString() //�׽�Ʈ�� ToString ������
    {
        return $"�ִ�ü�� : {maxHp}, ���� ü�� {hp}, �̵��ӵ� : {speed}, ���� �̵��ӵ� : {currentSpeed},\n" +
            $"����ġ �䱸�� : {maxExp}, ���� ����ġ : {exp}, ���� : {level}";
    }

    public float IncreaseExp(int value) //����ġ ���� �Լ�. value��ŭ ����ġ ����
    {
        int result = value + (value * expGainPercentage / 100);
        if (exp + result >= maxExp)
        {
            level++;
            exp = exp + result - maxExp;
            InGameManager.Instance.DLevelUp();
            //�ִ� ����ġ ������ ���� ���� ���� �߰��ؾ���
            maxExp += 100 * (level - 1);
        }
        else
        {
            exp += result;
        }
        return (float)exp / (float)maxExp;
    }
    public void Recovery(float value, EApplicableType type) //��ġ ���� Ÿ�Կ� ���� Hp ȸ��
    {
        if (hp >= currentMaxHp) return;
        hp += currentMaxHp * value / 100;
        if (hp > currentMaxHp) hp = currentMaxHp;
    }
    public void Recovery(float value)
    {
        if (hp >= currentMaxHp) return;
        hp += value;
        if (hp > currentMaxHp) hp = currentMaxHp;
    }
}
public enum EApplicableType
{
    None,
    Percentage
}
