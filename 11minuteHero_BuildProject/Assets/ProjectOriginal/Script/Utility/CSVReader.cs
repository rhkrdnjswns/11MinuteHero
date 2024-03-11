using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader : MonoBehaviour
{
    public enum EColumnIndex
    {
        SkillType = 1,
        SkillName,
        Description,
        BaseDamage,
        SecondDamage,
        CoolTime
    }

    public TextAsset csvFile;
    public List<string[]> skillData = new List<string[]>();

    // Start is called before the first frame update
    void Awake()
    {
        if (csvFile == null) Debug.LogError("읽어올 CSV파일이 존재하지 않습니다.\nCSV파일을 할당해주세요.");

        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] rowData = line.Split(',');

            skillData.Add(rowData);
        }
    }

    public ESkillType GetSkillType(int id)
    {
        if (id > skillData.Count) return 0;
        switch (skillData[id][(int)EColumnIndex.SkillType])
        {
            case "액티브":
                return ESkillType.Active;
            case "패시브":
                return ESkillType.Passive;
            case "진화":
                return ESkillType.Evolution;
            default:
                Debug.LogError("유효하지 않은 스킬 타입입니다.");
                return 0;
        }
    }
    public string[] GetSkillNameAndDescription(int id)
    {
        if (id > skillData.Count) return null;
        string[] strArr = new string[2];
        strArr[0] = skillData[id][(int)EColumnIndex.SkillName];
        strArr[1] = skillData[id][(int)EColumnIndex.Description];
        return strArr;
    }
    public float GetBaseDamage(int id)
    {
        if (id > skillData.Count) return -1;
        float f = 0;
        if(float.TryParse(skillData[id][(int)EColumnIndex.BaseDamage], out f))
        {
            return f;
        }
        Debug.LogError("문자열을 실수로 변환할 수 없습니다. CSV 파일의 값 또는 행을 확인해 주세요.");
        return -1;
    }
    public float GetCoolTime(int id)
    {
        if (id > skillData.Count) return -1;
        float f = 0;
        if (float.TryParse(skillData[id][(int)EColumnIndex.CoolTime], out f))
        {
            return f;
        }
        Debug.LogError("문자열을 실수로 변환할 수 없습니다. CSV 파일의 값 또는 행을 확인해 주세요.");
        return -1;
    }
}
