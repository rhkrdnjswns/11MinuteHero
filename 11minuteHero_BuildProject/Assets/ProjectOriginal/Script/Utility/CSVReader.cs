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

    public TextAsset[] csvFileArray;
    public List<List<string[]>> skillDataList = new List<List<string[]>>();

    private void Start()
    {
        StartCoroutine(Co_ReadCSVFile());
    }
    private IEnumerator Co_ReadCSVFile() //csvFileArray�� �ִ� CSV ���ϵ��� ���ڿ��� ��ȯ�Ͽ� ����Ʈ�� �о��
    {
        if (csvFileArray.Length < 1) Debug.LogError("�о�� CSV������ �������� �ʽ��ϴ�.\nCSV������ �Ҵ����ּ���.");
        for (int i = 0; i < csvFileArray.Length; i++)
        {
            StringReader reader = new StringReader(csvFileArray[i].text); //StringReaderŬ������ CSV������ string���� �о��
            while(reader.Peek() > -1)
            {
                string line = reader.ReadLine();
                string[] rowData = line.Split(',');
                List<string[]> strArrList = new List<string[]>();
                strArrList.Add(rowData);

                skillDataList.Add(strArrList);
                yield return null;
            }
        }
    }

    public string[] GetSkillNameAndDescription(ESkillType skillType, int id)
    {
        if (id > skillDataList.Count) return null;
        string[] strArr = new string[2];
        strArr[0] = skillDataList[(int)skillType][id][(int)EColumnIndex.SkillName];
        strArr[1] = skillDataList[(int)skillType][id][(int)EColumnIndex.Description];
        return strArr;
    }
    public float GetBaseDamage(ESkillType skillType, int id)
    {
        if (id > skillDataList.Count) return -1;
        if (skillType == ESkillType.Passive) return -1;
        float f = 0;
        if(float.TryParse(skillDataList[(int)skillType][id][(int)EColumnIndex.BaseDamage], out f))
        {
            return f;
        }
        Debug.LogError("���ڿ��� �Ǽ��� ��ȯ�� �� �����ϴ�. CSV ������ �� �Ǵ� ���� Ȯ���� �ּ���.");
        return -1;
    }
    public float GetCoolTime(ESkillType skillType, int id)
    {
        if (id > skillDataList.Count) return -1;
        if (skillType == ESkillType.Passive) return -1;
        float f = 0;
        if (float.TryParse(skillDataList[(int)skillType][id][(int)EColumnIndex.CoolTime], out f))
        {
            return f;
        }
        Debug.LogError("���ڿ��� �Ǽ��� ��ȯ�� �� �����ϴ�. CSV ������ �� �Ǵ� ���� Ȯ���� �ּ���.");
        return -1;
    }
}
