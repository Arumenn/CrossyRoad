﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [Header("GUI")]
    public Text uiCoin = null;
    public Text uiDistance = null;
    public Text uiCoinHighscore = null;
    public Text uiDistanceHighscore = null;
    public Camera _camera = null;
    public GameObject uiStartScreen = null;
    public GameObject uiGameOver = null;
    [Header("Level")]
    public LevelGenerator levelGenerator = null;
    public int levelCount = 50;
    public float outerLimitsX = 25f;
    public float outerLimitZ = -7.5f;
    public Light sun = null;
    public Light moon = null;

    [HideInInspector] public bool isNight = false;

    private int currentCoins = 0;
    private int currentDistance = 0;
    private bool canPlay = false;
    private int highscoreDistance = 0;
    private int highscoreCoins = 0;

    private static Manager s_Instance;
    public static Manager GetInstance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<Manager>() as Manager;
            }

            return s_Instance;
        }
    }

    private void Start()
    {
        highscoreDistance = PlayerPrefs.GetInt("HighscoreDistance", 0);
        highscoreCoins = PlayerPrefs.GetInt("HighscoreCoins", 0);
        uiDistanceHighscore.text = highscoreDistance.ToString();
        uiCoinHighscore.text = highscoreCoins.ToString();

        levelGenerator.SetupNewLevel();
        for (int i = 0; i < levelCount; i++)
        {
            levelGenerator.RandomGenerator();
        }
    }

    private void Update()
    {
        if (!CanPlay())
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (uiGameOver.active)
                {
                    PlayAgain();
                } else
                {
                    StartPlay();
                }
            }
        }
    }

    public void UpddateCoinCount(int value = 1)
    {
        currentCoins += value;
        uiCoin.text = currentCoins.ToString();
        if (currentCoins > highscoreCoins)
        {
            uiCoinHighscore.text = currentCoins.ToString();
        }
    }

    public void UpdateDistanceCount()
    {
        currentDistance++;
        uiDistance.text = currentDistance.ToString();
        if (currentDistance > highscoreDistance)
        {
            uiDistanceHighscore.text = currentDistance.ToString();
        }

        levelGenerator.RandomGenerator();
    }

    public bool CanPlay()
    {
        return canPlay;
    }

    public void StartPlay()
    {
        canPlay = true;
        uiStartScreen.SetActive(false);
    }

    public void GameOver()
    {
        canPlay = false;
        _camera.GetComponent<CameraShake>().Shake();
        _camera.GetComponent<CameraFollow>().enabled = false;
        GuiGameOver();
        //saves highscore if necessary
        if (currentDistance > highscoreDistance)
        {
            PlayerPrefs.SetInt("HighscoreDistance", currentDistance);
        }
        if (currentCoins > highscoreCoins)
        {
            PlayerPrefs.SetInt("HighscoreCoins", currentCoins);
        }
    }

    private void GuiGameOver()
    {
        Debug.Log("Game over!");
        uiGameOver.SetActive(true);
    }

    public void PlayAgain()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public bool IsOutsideLimit(Vector3 objPos, bool isMover)
    {
        if (Mathf.Abs(objPos.x) >= (isMover ? this.outerLimitsX + 10 : this.outerLimitsX))
        {
            return true;
        }
        if (objPos.z <= this.outerLimitZ)
        {
            return true;
        }
        return false;
    }
}
