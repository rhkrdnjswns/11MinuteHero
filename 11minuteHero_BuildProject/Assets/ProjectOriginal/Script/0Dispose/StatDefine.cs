using UnityEngine;

[System.Serializable]
public class CommonStats //모든 플레이어블 상호작용 객체의 공통 능력치 클래스
{
    [SerializeField] protected float hp;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float speed;
    [SerializeField] protected float currentSpeed; //현재 속도가 변경되는 수치. speed는 초기 고정 수치
    [SerializeField] protected float damage; //플레이어의 데미지는 무기와 기믹에 플러스로 작용함

    public CommonStats(float maxHp, float speed, float damage)
    {
        this.maxHp = maxHp;
        hp = maxHp;
        this.speed = speed;
        currentSpeed = speed;
        this.damage = damage;
    }

    //변동 수치는 전부 함수를 따로 만들지 않고 프로퍼티로 조정
    public float Speed { get => speed; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public float Damage { get => damage; }
    public float MaxHp { get => maxHp; }
    public float Hp { get => hp; }

    public virtual void DecreaseHp(float value) //hp 감소 함수. value만큼 감소시킴. 이후 체력바 갱신
    {
        hp -= value;
        if (hp < 0) hp = 0;
    }
}
[System.Serializable]
public class MonsterStats : CommonStats //몬스터의 능력치 클래스. 공통 능력치 클래스 상속
{
    private int rewardExp; //몬스터 처치 시 플레이어 경험치 상승량
    public MonsterStats(float maxHp, float speed, float damage, int rewardExp) : base(maxHp, speed, damage) //생성자 재정의
    {
        this.rewardExp = rewardExp;
    }
    public int GetRewardExp()
    {
        return rewardExp;
    }
    public void ResetStats() //몬스터 데이터 초기화
    {
        hp = maxHp;
    }
    public override void DecreaseHp(float value)
    {
        base.DecreaseHp(value);
    }
    public bool IsDie() //몬스터 사망 체크
    {
        return hp <= 0 ? true : false;
    }
}
[System.Serializable]
public class PlayerStats : CommonStats //플레이어의 능력치 클래스. 공통 능력치 클래스 상속
{
    [SerializeField] private int exp;
    [SerializeField] private int maxExp;
    [SerializeField] private int level;

    private float currentDamage; //오리지널 수치를 기반으로 증가하는 능력치의 경우 실적용은 current로 함
    private float currentMaxHp;

    //패시브 기믹에 대응하는 플레이어 능력치들
    private int expGainPercentage; //경험치 획득량
    private float reduceDamagePercentage; //피해 감소량


    //수치에 변경이 있는 변수는 전부 프로퍼티로 빼서 수치 변경. (따로 함수 안만듬)
    public float CurrentDamage { get => currentDamage; set => currentDamage = value; }
    public float CurrentMaxHp { get => currentMaxHp; set => currentMaxHp = value; }
    public int ExpGainPercentage { get => expGainPercentage; set => expGainPercentage = value; }
    public float ReduceDamagePercentage { get => reduceDamagePercentage; set => reduceDamagePercentage = value; }
    public int Level { get => level; }

    public PlayerStats(float maxHp, float speed, float damage, int maxExp) : base(maxHp, speed, damage)
    {
        //플레이어 스탯 초기화
        currentDamage = damage;
        currentMaxHp = maxHp;
        this.maxExp = maxExp;
        level = 1;
    }
    public override void DecreaseHp(float value) //체력 감소 함수 재정의. 플레이어는 피격 시 무적 효과를 주려고 재정의했음.
    {
        hp -= value - (value * reduceDamagePercentage / 100); //피해 감소량을 포함하여 연산
        if (hp < 0) hp = 0;
        if (hp <= 0) InGameManager.Instance.GameState = EGameState.GameOver; //플레이어 hp가 0이 되면 GameOver 
    }
    public override string ToString() //테스트용 ToString 재정의
    {
        return $"최대체력 : {maxHp}, 현재 체력 {hp}, 이동속도 : {speed}, 현재 이동속도 : {currentSpeed},\n" +
            $"경험치 요구량 : {maxExp}, 현재 경험치 : {exp}, 레벨 : {level}";
    }

    public float IncreaseExp(int value) //경험치 증가 함수. value만큼 경험치 증가
    {
        int result = value + (value * expGainPercentage / 100);
        if (exp + result >= maxExp)
        {
            level++;
            exp = exp + result - maxExp;
            InGameManager.Instance.DLevelUp();
            //최대 경험치 레벨에 따라 증가 로직 추가해야함
            maxExp += 100 * (level - 1);
        }
        else
        {
            exp += result;
        }
        return (float)exp / (float)maxExp;
    }
    public void Recovery(float value, EApplicableType type) //수치 적용 타입에 따라 Hp 회복
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
