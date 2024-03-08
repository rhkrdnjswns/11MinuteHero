using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnim : MonoBehaviour
{
    //public Transform loadingIcon;
    public Text text;
    //public float iconTime;
    public float textTime;
    private void Start()
    {
       // StartCoroutine(Co_IconAnim());
        StartCoroutine(Co_TextAnim());
    }
    //private IEnumerator Co_IconAnim()
    //{
    //    float timer = 0;
    //    int index = 0;
    //    WaitForSeconds waitFrame = new WaitForSeconds(ConstDefine.FRAME_DELAY);
    //    WaitForSeconds waitTime = new WaitForSeconds(iconTime);
    //    while(true)
    //    {
    //        loadingIcon.GetChild(index).gameObject.SetActive(false);
    //        index = index >= loadingIcon.childCount ? 0 : index++;
    //        loadingIcon.GetChild(index).gameObject.SetActive(true);
    //        yield return waitTime;
    //    }
    //}
    private IEnumerator Co_TextAnim()
    {
        WaitForSeconds waitTime = new WaitForSeconds(textTime);
        while (true)
        {
            if(text.text.Length < 7)
            {
                text.text += '.';
            }
            else
            {
                text.text = "·ÎµùÁß";
                yield return waitTime;
            }
            yield return waitTime;
        }
    }
}
