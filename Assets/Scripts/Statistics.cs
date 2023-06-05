using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Statistics : MonoBehaviour
{
    public TextMeshProUGUI winCountText;
    public TextMeshProUGUI gameCountText;
    public TextMeshProUGUI submittedRowCountText;
    public TextMeshProUGUI winRateText;

    private float winCount = 0f;
    private float gameCount = 0f;
    private int submittedRowCount = 0;
    private float winRate = 0;

    void Start()
    {
        LoadStatistics();
    }
    void Update()
    {
        winCountText.text = "Win count: " + winCount;
        gameCountText.text = "Total plays: " + gameCount;
        submittedRowCountText.text = "Entered numbers: " + submittedRowCount;
        winRateText.text = "Winrate: " + winRate + "%";
    }

    public void SaveStatistics()
    {
        PlayerPrefs.SetFloat("WinCount", winCount);
        PlayerPrefs.SetFloat("GameCount", gameCount);
        PlayerPrefs.SetInt("SubRowCount", submittedRowCount);
    }

    public void LoadStatistics()
    {
        if (PlayerPrefs.HasKey("WinCount"))
        {
            winCount = PlayerPrefs.GetFloat("WinCount");
            gameCount = PlayerPrefs.GetFloat("GameCount");
            submittedRowCount = PlayerPrefs.GetInt("SubRowCount");
        }
        CalculateWinRate();
    }

    public void ResetStatistics()
    {
        winCount = 0;
        PlayerPrefs.SetFloat("WinCount", winCount);
        gameCount = 0;
        PlayerPrefs.SetFloat("GameCount", gameCount);
        submittedRowCount = 0;
        PlayerPrefs.SetInt("SubRowCount", submittedRowCount);
        winRate = 0;
    }

    private void CalculateWinRate()
    {

        winRate = (winCount / (gameCount==0?1:gameCount)) * 100;
        winRate = (float)Math.Round(winRate, 2);
    }

    public void OnGameEnd(bool isWin)
    {
        gameCount++;
        if(isWin)
            winCount++;
        SaveStatistics();
        CalculateWinRate();
    }

    public void OnRowSubmit()
    {
        submittedRowCount++;
        CalculateWinRate();
    }
    
}
