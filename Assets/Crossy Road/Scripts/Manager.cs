﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public Text uiCoin = null;
    public Text uiDistance = null;
    public Camera _camera = null;
    public GameObject uiGameOver = null;

    private int currentCoins = 0;
    private int currentDistance = 0;
    private bool canPlay = false;

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
        //TODO Level generator
    }

    public void UpddateCoinCount(int value = 1)
    {
        currentCoins += value;
        uiCoin.text = currentCoins.ToString();
    }

    public void UpdateDistanceCount()
    {
        currentDistance++;
        uiDistance.text = currentDistance.ToString();

        //TODO generate new level piece here
    }

    public bool CanPlay()
    {
        return canPlay;
    }

    public void StartPlay()
    {
        canPlay = true;
    }

    public void GameOver()
    {
        canPlay = false;
        _camera.GetComponent<CameraShake>().Shake();
        _camera.GetComponent<CameraFollow>().enabled = false;
        GuiGameOver();
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
}