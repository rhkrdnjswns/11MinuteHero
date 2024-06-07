using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTest : MonoBehaviour
{
    public int index;

    public static GameTest Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }
    public void BtnEvt_SelectCharacter(int index)
    {
        this.index = index;
        //StartCoroutine(SceneUtility.TransitionScene(SceneInfo.InGame));
    }
}
