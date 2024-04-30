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
            throw new System.IndexOutOfRangeException("CSV 파일 배열의 인덱스를 벗어났습니다. CSV 파일 할당 여부를 확인하세요.");
        }
        
        string[] datas = ReadCSVData(csvIndex, rowIndex);

        if (typeof(T) == typeof(float) && float.TryParse(datas[columnIndex], out float f))
        {
            return (T)(object)f; //object타입으로 박싱 후 다시 제네릭 타입으로 박싱
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
        throw new System.Exception("CSV 파일 읽기에 실패했습니다. CSV 파일이 할당되어 있는지 확인하세요.");
    }
}

