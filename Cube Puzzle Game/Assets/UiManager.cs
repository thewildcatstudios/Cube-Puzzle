using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public GameObject NextLevelPanel, RestartPanel;
    public Text LevelText;
    PlayerMovements[] Players;
    public int[] PlayerIndex;

    void Start()
    {
        instance = this;

        int LevelNumber = SceneManager.GetActiveScene().buildIndex + 1;
        LevelText.text = "Level" + " " + LevelNumber;
    }

    void Update()
    {
        Players = GameObject.FindObjectsOfType<PlayerMovements>();

        for(int Count = 0; Count<Players.Length; Count++)
        {
            PlayerIndex[Count] = Players[Count].CubesCountsNumber;
        }
    }

    public void NextButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HintButton()
    {
        OneHintPlayer();
    }

    private void OneHintPlayer()
    {
        
    }
}
