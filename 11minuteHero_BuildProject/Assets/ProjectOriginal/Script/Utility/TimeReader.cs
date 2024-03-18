using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

// * 어플리케이션 종료 시 로컬 DB의 "마지막 접속 날짜"에 현재 날짜 정보(년/월/일)를 저장
// * 어플리케이션 시작 시 현재 날짜와 로컬 DB의 "마지막 접속 날짜"를 비교하여 다른 경우 일일 보상 획득 가능
public class TimeReader : MonoBehaviour
{
    public string url = "";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WebChk());
    }

    private IEnumerator WebChk()
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date");
                DateTime dateTime = DateTime.Parse(date);
                Debug.Log(dateTime);
                dateTime.ToUniversalTime();
                Debug.Log(dateTime);
            }
        }
    }
}

