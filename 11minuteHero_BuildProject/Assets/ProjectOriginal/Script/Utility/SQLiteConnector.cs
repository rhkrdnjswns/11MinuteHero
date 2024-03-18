using System.Collections;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.Threading.Tasks;
using System.IO;

// * 모든 스트림은 using문을 사용
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
            Debug.Log("로컬 저장소에 DB 생성 완료");
        }
        else
        {
            Debug.Log("이미 로컬 저장소에 DB가 존재합니다.");
        }
        CheckConection();
        ReadData();
        #region --폐기--
        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    //실제 어플을 사용할 플랫폼에서는 영구 보존 데이터 경로에서 DB가 존재하는지를 읽음
        //    targetPath = Path.Combine(Application.persistentDataPath, "TestDB.db"); //Application.persistentDataPath + "/Test.db"; 와 동일한 처리
        //    if (!File.Exists(targetPath))
        //    {
        //        yield return CopyFileAsync(filePath, targetPath);
        //        Debug.Log("Android/iOS 플랫폼의 로컬 저장소에 DB 생성 완료");
        //    }
        //    else
        //    {
        //        Debug.Log("이미 로컬 저장소에 DB가 존재합니다.");
        //    }
        //}
        //else
        //{
        //    targetPath = Path.Combine(Application.dataPath, "TestDB.db");
        //    if (!File.Exists(targetPath))
        //    {
        //        yield return CopyFileAsync(filePath, targetPath);
        //        Debug.Log("윈도우 플랫폼의 로컬 저장소에 DB 생성 완료");
        //    }
        //    else
        //    {
        //        Debug.Log("이미 로컬 저장소에 DB가 존재합니다.");
        //    }
        //}
        #endregion
    }
    private string GetDBPath() //DB파일의 경로 반환
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
    private string GetDBDataSourcePath() //DB파일의 경로에서 DB파일을 추출하여 반환
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
                    Debug.Log("데이터베이스가 연결되었습니다.");
                }
                else
                {
                    Debug.Log("데이터베이스 연결에 실패했습니다.");
                }
            }
        }             
        catch(Exception e)
        {
            Debug.LogError("DB 연결 중 오류 발생 : " + e.Message);
        }
    }
    private async Task CopyFileAsync(string filePath, string targetPath) //비동기로 targetPath 경로에 filePath 복사
    {
        try //파일 경로 등 문제가 있는 경우를 위한 예외 처리
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
            Debug.LogError("파일 복사 중 오류 발생 : " + e.Message);
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
