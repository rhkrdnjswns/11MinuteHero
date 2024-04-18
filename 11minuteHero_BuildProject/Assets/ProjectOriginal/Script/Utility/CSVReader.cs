using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader : MonoBehaviour
{
    public TextAsset[] csvFileArray;
    public List<object[]> skillDataList = new List<object[]>();

    private void Awake()
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
                object[] rowData = line.Split(',');
                for(int j = 0; j < rowData.Length; j++)
                {
                    if(rowData[j].ToString().Contains("."))
                    {
                        float f = 0;
                        if(float.TryParse(rowData[j].ToString(), out f))
                        {
                            rowData[j] = f;
                        }
                    }
                }
                skillDataList.Add(rowData);
                yield return null;
            }
        }
    }

    public string[] GetSkillData(int id)
    {
        return null;
    }
    //public string[] GetSkillNameAndDescription(ESkillType skillType, int id)
    //{
    //    if (id > skillDataList.Count) return null;
    //    string[] strArr = new string[2];
    //    strArr[0] = skillDataList[(int)skillType][id][(int)EColumnIndex.SkillName];
    //    strArr[1] = skillDataList[(int)skillType][id][(int)EColumnIndex.Description];
    //    return strArr;
    //}
    //public float GetBaseDamage(ESkillType skillType, int id)
    //{
    //    if (id > skillDataList.Count) return -1;
    //    if (skillType == ESkillType.Passive) return -1;
    //    float f = 0;
    //    if(float.TryParse(skillDataList[(int)skillType][id][(int)EColumnIndex.BaseDamage], out f))
    //    {
    //        return f;
    //    }
    //    Debug.LogError("���ڿ��� �Ǽ��� ��ȯ�� �� �����ϴ�. CSV ������ �� �Ǵ� ���� Ȯ���� �ּ���.");
    //    return -1;
    //}
    //public float GetCoolTime(ESkillType skillType, int id)
    //{
    //    if (id > skillDataList.Count) return -1;
    //    if (skillType == ESkillType.Passive) return -1;
    //    float f = 0;
    //    if (float.TryParse(skillDataList[(int)skillType][id][(int)EColumnIndex.CoolTime], out f))
    //    {
    //        return f;
    //    }
    //    Debug.LogError("���ڿ��� �Ǽ��� ��ȯ�� �� �����ϴ�. CSV ������ �� �Ǵ� ���� Ȯ���� �ּ���.");
    //    return -1;
    //}
}
