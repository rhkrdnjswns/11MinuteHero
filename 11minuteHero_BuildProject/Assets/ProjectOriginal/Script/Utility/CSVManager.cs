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
    private void ReadCSVFile() //csvFileArray에 있는 CSV 파일들을 문자열로 변환하여 리스트에 읽어옴
    {
        if (csvFileArray.Length < 1) Debug.LogError("읽어올 CSV파일이 존재하지 않습니다.\nCSV파일을 할당해주세요.");

        for (int i = 0; i < csvFileArray.Length; i++)
        {
            StringReader reader = new StringReader(csvFileArray[i].text); //StringReader클래스로 CSV파일을 string으로 읽어옴
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
            return (T)(object)f; //object타입으로 박싱 후 다시 제네릭 타입으로 박싱
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
            throw new System.Exception("유효하지 않은 타입");
        }
    }
}

