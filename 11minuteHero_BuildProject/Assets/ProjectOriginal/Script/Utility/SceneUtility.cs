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

public class SceneUtility
{
    public static IEnumerator TransitionScene(SceneInfo sceneInfo)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)sceneInfo);
        operation.allowSceneActivation = true;
        yield return null;
    }
}
