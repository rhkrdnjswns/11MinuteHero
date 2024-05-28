using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int characterIndex;
    public int stageIndex;
    public int difficultyIndex;

    public int sceneIndex { get; private set; }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(instance);
    }
    public void LoadScene(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
        SceneManager.LoadScene((int)SceneInfo.Loading);
    }
}
