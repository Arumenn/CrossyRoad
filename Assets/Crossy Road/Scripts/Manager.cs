using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Manager : MonoBehaviour
{
    [Header("Game")]
    public bool multiplayer = false;
    public bool paused = false;
    public bool gameOver = false;

    [Header("GUI")]
    [Header("SinglePlayer")]
    public Text uiCoin = null;
    public Text uiDistance = null;
    public Text uiCoinHighscore = null;
    public Text uiDistanceHighscore = null;
    public GameObject singleUIScreen = null;
    [Header("Player1")]
    public Text player1UiCoin = null;
    public Text player1UiDistance = null;
    public Text player1UiCoinHighscore = null;
    public Text player1UiDistanceHighscore = null;
    public GameObject player1UIScreen = null;
    [Header("Player2")]
    public Text player2UiCoin = null;
    public Text player2UiDistance = null;
    public Text player2UiCoinHighscore = null;
    public Text player2UiDistanceHighscore = null;
    public GameObject player2UIScreen = null;
    [Header("CameraStuff")]
    public Camera _camera = null;
    public Camera _cameraP1 = null;
    public Camera _cameraP2 = null;
    public Camera _cameraUI = null;
    public Camera _cameraP1UI = null;
    public Camera _cameraP2UI = null;
    [Header("Common UI")]
    public GameObject uiStartScreenSingle = null;
    public GameObject uiGameOver = null;
    public GameObject uiPause = null;
    [Header("Level")]
    public LevelGenerator levelGenerator = null;
    public int levelCount = 50;
    public float outerLimitsX = 25f;
    public float outerLimitZ = -7.5f;
    public Light sun = null;
    public Light moon = null;

    [HideInInspector] public bool isNight = false;

    private bool canPlay = false;

    private PlayerStats player1Stats = new PlayerStats();
    private PlayerStats player2Stats = new PlayerStats();
    public PlayerController player1 = null;
    public PlayerController player2 = null;

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

    public void Setup()
    {
        if (multiplayer)
        {
            player1Stats.highscoreDistance = PlayerPrefs.GetInt("HighscoreDistance_P1", 0);
            player1Stats.highscoreCoins = PlayerPrefs.GetInt("HighscoreCoins_P1", 0);
            player2Stats.highscoreDistance = PlayerPrefs.GetInt("HighscoreDistance_P2", 0);
            player2Stats.highscoreCoins = PlayerPrefs.GetInt("HighscoreCoins_P2", 0);
        } else
        {
            player1Stats.highscoreDistance = PlayerPrefs.GetInt("HighscoreDistance", 0);
            player1Stats.highscoreCoins = PlayerPrefs.GetInt("HighscoreCoins", 0);
        }
        UpdateHighscores();

        levelGenerator.SetupNewLevel();
        for (int i = 0; i < levelCount; i++)
        {
            levelGenerator.RandomGenerator();
        }

        //Multiplayer
        _camera.enabled = !multiplayer;
        _cameraP1.enabled = multiplayer;
        _cameraP2.enabled = multiplayer;
        _cameraUI.enabled = !multiplayer;
        _cameraP1UI.enabled = multiplayer;
        _cameraP2UI.enabled = multiplayer;
        singleUIScreen.SetActive(!multiplayer);
        player1UIScreen.SetActive(multiplayer);
        player2UIScreen.SetActive(multiplayer);

        player1.Setup();
        player2.Setup();
    }

    private void UpdateHighscores()
    {
        //Coins
        player1UiCoin.text = player1Stats.currentCoins.ToString();
        uiCoin.text = player1Stats.currentCoins.ToString();
        if (player1Stats.currentCoins > player1Stats.highscoreCoins)
        {
            player1UiCoinHighscore.text = player1Stats.currentCoins.ToString();
            uiCoinHighscore.text = player1Stats.currentCoins.ToString();
        } else
        {
            player1UiCoinHighscore.text = player1Stats.highscoreCoins.ToString();
            uiCoinHighscore.text = player1Stats.highscoreCoins.ToString();
        }

        if (multiplayer)
        {
            player2UiCoin.text = player2Stats.currentCoins.ToString();
            if (player2Stats.currentCoins > player2Stats.highscoreCoins)
            {
                player2UiCoinHighscore.text = player2Stats.currentCoins.ToString();
            } else
            { 
                player2UiCoinHighscore.text = player2Stats.highscoreCoins.ToString();
            }
        }

        //Distance
        player1UiDistance.text = player1Stats.currentDistance.ToString();
        uiDistance.text = player1Stats.currentDistance.ToString();
        if (player1Stats.currentDistance > player1Stats.highscoreDistance)
        {
            player1UiDistanceHighscore.text = player1Stats.currentDistance.ToString();
            uiDistanceHighscore.text = player1Stats.currentDistance.ToString();
        } else
        {
            player1UiDistanceHighscore.text = player1Stats.highscoreDistance.ToString();
            uiDistanceHighscore.text = player1Stats.highscoreDistance.ToString();
        }
        if (multiplayer)
        {
            player2UiDistance.text = player2Stats.currentDistance.ToString();
            if (player2Stats.currentDistance > player2Stats.highscoreDistance)
            {
                player2UiDistanceHighscore.text = player2Stats.currentDistance.ToString();
            } else
            {
                player2UiDistanceHighscore.text = player2Stats.highscoreDistance.ToString();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!gameOver)
            {
                StartPlay();
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (paused)
            {
                uiPause.SetActive(false);
                canPlay = true;
                Time.timeScale = 1;
                paused = false;
            } else
            {
                uiPause.SetActive(true);
                canPlay = false;
                Time.timeScale = 0;
                paused = true;
            }
        }
    }

    public void UpddateCoinCount(string player, int value = 1)
    {
        if (player == "P1")
        {
            player1Stats.currentCoins += value;
        } else
        {
            player2Stats.currentCoins += value;
            
        }
        UpdateHighscores();
    }

    public void UpdateDistanceCount(string player, float value)
    {
        if (player == "P1")
        {
            player1Stats.currentDistance += (int) value;
            if (player1Stats.currentDistance < 0) { player1Stats.currentDistance = 0;  }
        } else
        {
            player2Stats.currentDistance += (int) value;
            if (player2Stats.currentDistance < 0) { player2Stats.currentDistance = 0; }
        }
        UpdateHighscores();

        levelGenerator.RandomGenerator();
    }

    public bool CanPlay()
    {
        return canPlay;
    }

    public void StartPlay()
    {
        canPlay = true;
        uiStartScreenSingle.SetActive(false);
    }

    public bool LoseCondition(PlayerController playerWhoDied)
    {
        if (gameOver) { return true;  }
        if (multiplayer)
        {
            bool otherPlayerIsDeadToo = false;
            if (playerWhoDied.controllerPrefix == "P1")
            {
                otherPlayerIsDeadToo = player2.isDead;
            } else
            {
                otherPlayerIsDeadToo = player1.isDead;
            }
            if (otherPlayerIsDeadToo)
            {
                GameOver();
                return true;
            } else
            {
                if (!playerWhoDied.isRespawning)
                {
                    playerWhoDied.Respawn();
                }
                return false;
            }
        } else
        {
            GameOver();
            return true;
        }
    }

    public void GameOver()
    {
        canPlay = false;
        _camera.GetComponent<CameraShake>().Shake();
        _camera.GetComponent<CameraFollow>().enabled = false;
        GuiGameOver();
        //saves highscore if necessary
        if (multiplayer)
        {

        } else
        {

            if (player1Stats.currentDistance > player1Stats.highscoreDistance)
            {
                PlayerPrefs.SetInt("HighscoreDistance", player1Stats.currentDistance);
            }
            if (player1Stats.currentCoins > player1Stats.highscoreCoins)
            {
                PlayerPrefs.SetInt("HighscoreCoins", player1Stats.currentCoins);
            }
        }
    }

    private void GuiGameOver()
    {
        //Debug.Log("Game over!");
        uiGameOver.SetActive(true);
        gameOver = true;
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
