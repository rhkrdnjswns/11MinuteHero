using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface ICSVInitializing
{ 
    public void InitCSVData();
}

public enum ECommonSkillColumnIndex
{
    ID,
    SkillType,
    SkillName,
    Description
}
public enum EActiveSkillColumnIndex
{
    SensingRange = 4,
    BaseDamage,
    SecondDamage,
    CoolTime,
    DamageCycle,
    DamageCount,
    MaxMobCount,
    AttactRange,
    LevelUpAttactRangeIncrease,
    ProjectileCount,
    MultiProjectileCount,
    PlusProjectileCount,
    LevelUpProjectileRangeIncrease,
    ProjectileRemoveTime,
    ProjectileSpeed,
    ProjectileLifeCycle,
    ProjectilePenetrateCount,
    PlusProjectilePenetrateCount,
    ProjectileRotationSpeed,
    ProjectileCreateDistance,
    ProjectileMaxMoveDistance,
    ProjectileExplosionRange,
    FireAreaRange,
    FireAreaRemoveTime,
    ProjectileCreateTime,
    ProjectileSensingRange,
    PlusProjectileBounceCount,
    MobSpeedDecrease,
    MobSpeedDecreaseTime
}
public enum EPassiveSkillColumnIndex
{
    ValuePercentage = 4,
    LevelUpIncreaseValuePercentage
}
public enum EEvolutionSkillColumnIndex
{
    SensingRange = 4,
    BaseDamage,
    CoolTime,
    DamageCycle,
    DamageCount,
    MaxMobCount,
    AttactRange,
    ProjectileCount,
    ProjectileRemoveTime,
    ProjectileSpeed,
    ProjectileLifeCycle,
    ProjectilePenetrateCount,
    ProjectileRotationSpeed,
    ProjectileCreateDistance,
    ProjectileMaxMoveDistance,
    ProjectileExplosionRange,
    FireAreaRange,
    FireAreaRemoveTime,
    ProjectileCreateTime,
    ProjectileSensingRange,
    MobSpeedDecrease,
    MobSpeedDecreaseTime,
    PlayerHpIncreaseCount,
    PlayerIncreaseMoveSpeed,
    PlayerIncreaseMoveSpeedCount,
    PlayerIncreaseMoveSpeedTime,
    PlayerHpRecovery,
    MobFaintTime
}


public class CSVManager : MonoBehaviour
{
    public TextAsset[] csvFileArray;
    public List<List<string[]>> skillDataList = new List<List<string[]>>();
    public readonly string nullSquare = "null";

    private void Awake()
    {
        ReadCSVFile();
    }
    private void ReadCSVFile() //csvFileArray�� �ִ� CSV ���ϵ��� ���ڿ��� ��ȯ�Ͽ� ����Ʈ�� �о��
    {
        if (csvFileArray.Length < 1) Debug.LogError("�о�� CSV������ �������� �ʽ��ϴ�.\nCSV������ �Ҵ����ּ���.");

        for (int i = 0; i < csvFileArray.Length; i++)
        {
            StringReader reader = new StringReader(csvFileArray[i].text); //StringReaderŬ������ CSV������ string���� �о��
            List<string[]> datas = new List<string[]>();
            while (reader.Peek() > -1)
            {
                string line = reader.ReadLine();
                string[] rowData = line.ToString().Split(',');

                datas.Add(rowData);
            }
            skillDataList.Add(datas);
        }
    }
    public T GetSKillCSVData<T>(ESkillType skillType, int rowIndex, int columnIndex)
    {
        int index = (int)skillType;
        if (skillDataList[index][rowIndex][columnIndex] == nullSquare)
        {
            return default(T);
        }
        if (typeof(T) == typeof(float) && float.TryParse(skillDataList[index][rowIndex][columnIndex], out float f))
        {
            return (T)(object)f; //objectŸ������ �ڽ� �� �ٽ� ���׸� Ÿ������ �ڽ�
        }
        if (typeof(T) == typeof(int) && int.TryParse(skillDataList[index][rowIndex][columnIndex], out int i))
        {
            return (T)(object)i;
        }
        if (typeof(T) == typeof(string))
        {
            return (T)(object)skillDataList[index][rowIndex][columnIndex];
        }
        else
        {
            throw new System.Exception("��ȿ���� ���� Ÿ��");
        }
    }
}

