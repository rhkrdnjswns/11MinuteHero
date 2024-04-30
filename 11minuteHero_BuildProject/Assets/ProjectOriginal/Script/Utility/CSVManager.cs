using UnityEngine;
using System.IO;

public class CSVManager : MonoBehaviour
{
    public TextAsset[] csvFileArray;

    public T GetCSVData<T>(int csvIndex, int rowIndex, int columnIndex)
    {
        if (csvIndex == 1) rowIndex = rowIndex - 17;
        if (csvIndex == 2) rowIndex = rowIndex - 26;

        if (csvIndex >= csvFileArray.Length)
        {
            throw new System.IndexOutOfRangeException("CSV ���� �迭�� �ε����� ������ϴ�. CSV ���� �Ҵ� ���θ� Ȯ���ϼ���.");
        }
        
        string[] datas = ReadCSVData(csvIndex, rowIndex);

        if (typeof(T) == typeof(float) && float.TryParse(datas[columnIndex], out float f))
        {
            return (T)(object)f; //objectŸ������ �ڽ� �� �ٽ� ���׸� Ÿ������ �ڽ�
        }
        if (typeof(T) == typeof(int) && int.TryParse(datas[columnIndex], out int i))
        {
            return (T)(object)i;
        }
        if (typeof(T) == typeof(string))
        {
            return (T)(object)datas[columnIndex];
        }
        else
        {
            return default(T);
        }
    }
    private string[] ReadCSVData(int index, int rowIndex)
    {
        using (StringReader reader = new StringReader(csvFileArray[index].text))
        {
            string line = string.Empty;

            line = reader.ReadLine();
            
            for (int i = 0; i < rowIndex; i++)
            {
                line = reader.ReadLine();
            }
            string[] rowData = line.Split(',');
            for(int i = 0; i < rowData.Length; i++)
            {
                if (rowData[i] == string.Empty)
                {
                    rowData[i] = default(string);
                }
            }
            return rowData;
        }
        throw new System.Exception("CSV ���� �б⿡ �����߽��ϴ�. CSV ������ �Ҵ�Ǿ� �ִ��� Ȯ���ϼ���.");
    }
}

