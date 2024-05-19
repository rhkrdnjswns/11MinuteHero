using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopUp : MonoBehaviour
{
    [SerializeField] private Text clearResultText;
    [SerializeField] private Text stageNameText;
    [SerializeField] private Text stageTimerText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private Text currentScoreText;
    [SerializeField] private Text killCountText;
    [SerializeField] private Text resultGoldText;
    

    public void SetResultPopUp(bool isClear, string stageName, string time, string score, string killCount)
    {
        clearResultText.text = isClear ? "Stage Clear" : "Game Over";
        stageNameText.text = stageName;
        stageTimerText.text = time;
        //bestScoreText.text = 최고점수 불러오기
        currentScoreText.text = score;
        killCountText.text = killCount;
        //resultColdText.text = 보상 골드 책정
    }
}
