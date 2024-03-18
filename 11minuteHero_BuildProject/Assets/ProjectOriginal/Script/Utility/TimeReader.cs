using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

// * ���ø����̼� ���� �� ���� DB�� "������ ���� ��¥"�� ���� ��¥ ����(��/��/��)�� ����
// * ���ø����̼� ���� �� ���� ��¥�� ���� DB�� "������ ���� ��¥"�� ���Ͽ� �ٸ� ��� ���� ���� ȹ�� ����
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

