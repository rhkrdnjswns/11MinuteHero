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
        if (csvFile == null) Debug.LogError("�о�� CSV������ �������� �ʽ��ϴ�.\nCSV������ �Ҵ����ּ���.");

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
            case "��Ƽ��":
                return ESkillType.Active;
            case "�нú�":
                return ESkillType.Passive;
            case "��ȭ":
                return ESkillType.Evolution;
            default:
                Debug.LogError("��ȿ���� ���� ��ų Ÿ���Դϴ�.");
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
        Debug.LogError("���ڿ��� �Ǽ��� ��ȯ�� �� �����ϴ�. CSV ������ �� �Ǵ� ���� Ȯ���� �ּ���.");
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
        Debug.LogError("���ڿ��� �Ǽ��� ��ȯ�� �� �����ϴ�. CSV ������ �� �Ǵ� ���� Ȯ���� �ּ���.");
        return -1;
    }
}
