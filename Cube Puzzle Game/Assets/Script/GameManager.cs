using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instans;
    public PlayerMovements[] Players;
    public int PlayerCubesInLevels;
    public int[] GameWinCounts = {0,0,0,0,0,0,0,0,0,0};
    public int CountsCubes;
    public bool WinGame , GameOver;
    public ParticleSystem WinEffect;
    bool WinEffectStart;
    public int FPS;


    void Start()
    {
        Instans = this;

        WinEffect = GameObject.Find("Win Effect").GetComponent<ParticleSystem>();

        PlayerPrefs.DeleteKey("CollosonCounter");

        Players = GameObject.FindObjectsOfType<PlayerMovements>();

        PlayerPrefs.SetInt("NumberOfCubesInLevel", Players.Length);

        for(int Number = 0; Number<Players.Length; Number++)
        {
            PlayerPrefs.DeleteKey("Cube(" + Number + ")");
        }

        PlayerCubesInLevels = Players.Length;




        QualitySettings.vSyncCount = 0;

        FPS = 800;
    }

    // Update is called once per frame
    void Update()
    {

       if(FPS != Application.targetFrameRate)
        {
            Application.targetFrameRate = FPS ;
        }


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
            Invoke("PlayerWinLevel", 2f);
        }

        if(PlayerPrefs.GetInt("CollosonCounter") == PlayerCubesInLevels - 1)
        {
            if(WinGame == false)
            {
               UiManager.instance.RestartPanel.SetActive(true);
            }
        }

        if(GameOver == true)
        {
            if (WinGame == false)
            {
                UiManager.instance.RestartPanel.SetActive(true);
            }
        }
    }

    public void PlayerWinLevel()
    {
        UiManager.instance.NextLevelPanel.SetActive(true);
    }
}
