using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public GameObject[] characterArray;
    public Image fadeImage;

    private GameObject currentCharacter;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetActiveCharacter(GameManager.instance.characterIndex);
        StartCoroutine(TweeningUtility.FadeOut(2f, fadeImage));
    }
    public void SetActiveCharacter(int index)
    {
        if(currentCharacter == characterArray[index])
        {
            return;
        }

        if (currentCharacter != null)
        {
            currentCharacter.SetActive(false);
        }

        characterArray[index].SetActive(true);
        currentCharacter = characterArray[index];
    }
    public void StartStage(int index, int difficulty)
    {
        GameManager.instance.stageIndex = index;
        GameManager.instance.difficultyIndex = difficulty;

        StartCoroutine(SceneUtility.TransitionScene(SceneInfo.InGame));
    }


}
