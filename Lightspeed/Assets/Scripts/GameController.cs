using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public GameObject Player;
    [HideInInspector] public bool curLevelIsFinished = false;

    private static string[] levels = { "level1", "level2" };
    private SaveData data;
    private float timer = 0f;

    void Awake()
    {
        if      (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }

        DontDestroyOnLoad(transform.gameObject);

        data = SaveSystem.LoadPlayer();
        if(data == null)
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
        int lightsCollectedPlaceholder = 0; //TODO
        if (data.levelTimes[data.currentLevel] > timer) data.UpdateDataAt(timer, lightsCollectedPlaceholder, data.currentLevel);
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
    }

    public void PlayCurrentLevel()
    {
        curLevelIsFinished = false;
        SceneManager.LoadScene(levels[data.currentLevel]);
        Player = GameObject.Find("Player");
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

    public int GetNumLevels() { return levels.Length; }
}
