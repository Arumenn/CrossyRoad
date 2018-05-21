using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MetaManager : MonoBehaviour {

    private bool created = false;
    private bool alreadyLoadedGameOnce = false;

    public string nomPlayer1 = "J1";
    public string nomPlayer2 = "J2";

    private static MetaManager s_Instance;
    public static MetaManager GetInstance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<MetaManager>() as MetaManager;
            }

            return s_Instance;
        }
    }

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            Debug.Log("Awake: " + this.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartSolo();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GotoVersusMenu();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Quit();
            }
        } else if (SceneManager.GetActiveScene().name == "TheGame")
        {
            if (Manager.GetInstance.paused)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    PlayAgain();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    GotoMainMenu();
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Quit();
                }
            }
        }else if (SceneManager.GetActiveScene().name == "VersusMenu")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UpdatePlayerNames();
                StartCoroutine(LoadGame(true));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GotoMainMenu();
            }
        }
	}

    IEnumerator LoadGame(bool multiplayer)
    {
        SceneManager.LoadScene("Loading");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("TheGame");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Manager.GetInstance.multiplayer = multiplayer;
        Manager.GetInstance.Setup();
        alreadyLoadedGameOnce = true;
    }

    public void PlayAgain()
    {
        Debug.Log("Restarting...");
        StartCoroutine(LoadGame(Manager.GetInstance.multiplayer));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartSolo()
    {
        Debug.Log("Starting solo...");
        StartCoroutine(LoadGame(false));
    }

    public void GotoVersusMenu()
    {
        Debug.Log("Going to Versus...");
        SceneManager.LoadScene("VersusMenu");
    }

    public void GotoMainMenu()
    {
        Debug.Log("Going to MainMenu...");
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdatePlayerNames()
    {
        nomPlayer1 = "J1";
        nomPlayer2 = "J2";

        string p1 = "";
        string p2 = "";

        foreach (InputField i in FindObjectsOfType<InputField>())
        {
            if (i.name == "Player1Name")
            {
                p1 = i.text;
            }else if (i.name == "Player2Name")
            {
                p2 = i.text;
            }
        }

        if (p1.Length > 0) { nomPlayer1 = p1; }
        if (p2.Length > 0) { nomPlayer2 = p2; }
    }
}
