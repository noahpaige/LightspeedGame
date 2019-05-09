using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public GameObject Player;
    [HideInInspector] public bool curLevelIsFinished = false;

    private static string[] levels = {"level1", "level2", "level3"};
    private string mainMenu = "MainMenu";
    private SaveData data;
    private float timer = 0f;

    void Awake()
    {
        if      (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }

        DontDestroyOnLoad(transform.gameObject);

        //######################################### uncomment before release#################################################
        //data = SaveSystem.LoadPlayer();    
        data = SaveSystem.CreateNewSaveData();


        if (data == null)
        {
            Debug.Log("Creating new save data.");
            data = SaveSystem.CreateNewSaveData();
        }        
    }

    private void Update()
    {
        if (Player == null) Player = GameObject.Find("Player");
        if (!curLevelIsFinished) timer += Time.deltaTime;
    }

    public void PlayerWon()
    {
        curLevelIsFinished = true;
        Player.SetActive(false);

        int   lightsCollected = Player.transform.Find("LightContainer").GetComponent<LightContainerController>().GetLightCount();
        float bestTime        = timer;

        bool collectedMoreLights = data.lightsCollectedPerLevel[data.currentLevel] < lightsCollected;
        bool fasterThanPrevious  = data.levelTimes[data.currentLevel]              > timer;

        if (!collectedMoreLights) lightsCollected = data.lightsCollectedPerLevel[data.currentLevel];
        if (!fasterThanPrevious)  bestTime        = data.levelTimes[data.currentLevel];

        data.UpdateDataAt(timer, lightsCollected, data.currentLevel);
        timer = 0;
        Save();
    }

    public void GoToNextScene()
    {
        curLevelIsFinished = false;
        data.currentLevel = (data.currentLevel + 1) % levels.Length;
        data.maxLevelReached = Mathf.Max(data.maxLevelReached, data.currentLevel);
        SceneManager.LoadScene(levels[data.currentLevel]);
        Player = GameObject.Find("Player");
        timer = 0;
    }

    public void GoToMainMenu()
    {
        curLevelIsFinished = false;
        data.maxLevelReached = Mathf.Max(data.maxLevelReached, data.currentLevel);
        SceneManager.LoadScene(mainMenu);
    }

    public void PlayCurrentLevel()
    {
        curLevelIsFinished = false;
        SceneManager.LoadScene(levels[data.currentLevel]);
        Player = GameObject.Find("Player");
        timer = 0;
    }

    public void Save()
    {
        Debug.Log("Saving Game...");
        Debug.Log("         Current Level: " + levels[data.currentLevel]);
        Debug.Log("         Max     Level: " + levels[data.maxLevelReached]);
        for (int i = 0; i < levels.Length; i++)
        {
            Debug.Log("                 Level Time: " + data.levelTimes[i]);
        }
        Debug.Log("... Game Saved");
        
        SaveSystem.SavePlayerData(data);
    }

    public void ClearSaveData()
    {
        Debug.Log("Clearing Save Data...");
        data = SaveSystem.CreateNewSaveData();
        Debug.Log("... Save Data Cleared.");

    }

    public int GetNumLevels() { return levels.Length; }

    public SaveData GetSaveData()
    {
        SaveData returnme = data;
        return returnme;
    }

    public void GoToGivenLevel(int level)
    {
        SceneManager.LoadScene(levels[level - 1]);
        Player = GameObject.Find("Player");
        timer = 0;
    }
}
