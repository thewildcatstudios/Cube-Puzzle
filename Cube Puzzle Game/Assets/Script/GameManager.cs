using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instans;

    public PlayerMovements[] Players;
    public GameObject NextLevelPanel , RestartPanel;
    public int PlayerCubesInLevels;
    public int[] GameWinCounts = {0,0,0,0,0,0,0,0,0,0};
    public int CountsCubes;
    public bool WinGame , GameOver;
    public ParticleSystem WinEffect;
    bool WinEffectStart;


    [Header("----------test----------")]
    public Text LevelText;

    void Start()
    {
        Instans = this;

        PlayerPrefs.DeleteKey("AmountOfYaxis");
        PlayerPrefs.DeleteKey("CollosonCounter");

        Players = GameObject.FindObjectsOfType<PlayerMovements>();

        PlayerPrefs.SetInt("NumberOfCubesInLevel", Players.Length);

        for(int Number = 0; Number<Players.Length; Number++)
        {
            PlayerPrefs.DeleteKey("Cube(" + Number + ")");
        }

        PlayerCubesInLevels = Players.Length;

        int LevelNumber = SceneManager.GetActiveScene().buildIndex + 1;
        LevelText.text = "Level"+ " " + LevelNumber;

    }

    // Update is called once per frame
    void Update()
    {
        for (int Number = 0; Number < PlayerCubesInLevels; Number++)
        {
            if(PlayerPrefs.GetInt("Cube(" + Number + ")") == 1)
            {
                if(GameWinCounts[Number] == 0)
                {
                    GameWinCounts[Number] = 1;
                    CountsCubes++;
                }
            }
        }


        if(CountsCubes == PlayerCubesInLevels - 1)
        {
            WinGame = true;
            if(WinEffectStart == false)
            {
                WinEffect.Play();
                WinEffectStart = true;
            }
            Invoke("PlayerWinLevel", 3f);
        }

        if(PlayerPrefs.GetInt("CollosonCounter") == PlayerCubesInLevels - 1)
        {
            if(WinGame == false)
            {
                RestartPanel.SetActive(true);
            }
        }

        if(GameOver == true)
        {
            if (WinGame == false)
            {
                RestartPanel.SetActive(true);
            }
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

    public void PlayerWinLevel()
    {
        NextLevelPanel.SetActive(true);
    }
}
