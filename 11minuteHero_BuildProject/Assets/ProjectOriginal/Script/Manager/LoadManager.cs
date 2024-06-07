using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneInfo
{
    Title,
    Loading,
    Main,
    InGame
}

public class LoadManager : MonoBehaviour
{
    private bool bLoadComplete;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(Co_LoadSceneAsync(GameManager.instance.sceneIndex));
    }
    private IEnumerator Co_LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }
}
