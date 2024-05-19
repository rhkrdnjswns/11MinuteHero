using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    private bool bLoadComplete;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        //yield return new WaitUntil(() => bLoadComplete);
        StartCoroutine(SceneUtility.TransitionScene(SceneInfo.Main));
    }
}
