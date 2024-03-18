using System.Collections;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.Threading.Tasks;
using System.IO;

// * ��� ��Ʈ���� using���� ���
public class SQLiteConnector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_CreateDB());
    }
    private IEnumerator Co_CreateDB()
    {
        string targetPath = GetDBPath();
        string filePath = Path.Combine(Application.streamingAssetsPath, "TestDB.db");

        if (!File.Exists(targetPath))
        {
            yield return CopyFileAsync(filePath, targetPath);
            Debug.Log("���� ����ҿ� DB ���� �Ϸ�");
        }
        else
        {
            Debug.Log("�̹� ���� ����ҿ� DB�� �����մϴ�.");
        }
        CheckConection();
        ReadData();
        #region --���--
        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    //���� ������ ����� �÷��������� ���� ���� ������ ��ο��� DB�� �����ϴ����� ����
        //    targetPath = Path.Combine(Application.persistentDataPath, "TestDB.db"); //Application.persistentDataPath + "/Test.db"; �� ������ ó��
        //    if (!File.Exists(targetPath))
        //    {
        //        yield return CopyFileAsync(filePath, targetPath);
        //        Debug.Log("Android/iOS �÷����� ���� ����ҿ� DB ���� �Ϸ�");
        //    }
        //    else
        //    {
        //        Debug.Log("�̹� ���� ����ҿ� DB�� �����մϴ�.");
        //    }
        //}
        //else
        //{
        //    targetPath = Path.Combine(Application.dataPath, "TestDB.db");
        //    if (!File.Exists(targetPath))
        //    {
        //        yield return CopyFileAsync(filePath, targetPath);
        //        Debug.Log("������ �÷����� ���� ����ҿ� DB ���� �Ϸ�");
        //    }
        //    else
        //    {
        //        Debug.Log("�̹� ���� ����ҿ� DB�� �����մϴ�.");
        //    }
        //}
        #endregion
    }
    private string GetDBPath() //DB������ ��� ��ȯ
    {
        string path = string.Empty;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            path = Path.Combine(Application.persistentDataPath, "TestDB.db");
        }
        else
        {
            path = Path.Combine(Application.dataPath, "TestDB.db");
        }
        return path;
    }
    private string GetDBDataSourcePath() //DB������ ��ο��� DB������ �����Ͽ� ��ȯ
    {
        string path = string.Empty;
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            path = "Data Source=" + Path.Combine(Application.persistentDataPath, "TestDB.db");
        }
        else
        {
            path = "Data Source=" + Path.Combine(Application.dataPath, "TestDB.db");
        }
        return path;
    }
    private void CheckConection()
    {
        try
        {
            using (IDbConnection dbConnection = new SqliteConnection(GetDBDataSourcePath()))
            {
                dbConnection.Open();

                if (dbConnection.State == ConnectionState.Open)
                {
                    Debug.Log("�����ͺ��̽��� ����Ǿ����ϴ�.");
                }
                else
                {
                    Debug.Log("�����ͺ��̽� ���ῡ �����߽��ϴ�.");
                }
            }
        }             
        catch(Exception e)
        {
            Debug.LogError("DB ���� �� ���� �߻� : " + e.Message);
        }
    }
    private async Task CopyFileAsync(string filePath, string targetPath) //�񵿱�� targetPath ��ο� filePath ����
    {
        try //���� ��� �� ������ �ִ� ��츦 ���� ���� ó��
        {
            using (FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                using (FileStream targetStream = File.Create(targetPath))
                {
                    await fileStream.CopyToAsync(targetStream);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("���� ���� �� ���� �߻� : " + e.Message);
        }
    }

    public void ReadData()
    {
        using(IDbConnection dbConnection = new SqliteConnection(GetDBDataSourcePath()))
        {
            dbConnection.Open();
            using(IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = "Select GOLD From Test";
                using(IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    Debug.Log(dataReader.GetValue(dataReader.FieldCount - 1));
                }
            }
        }
    }
}
