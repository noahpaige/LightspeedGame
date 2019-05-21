using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {

    public static GameController instance;
    [HideInInspector] public bool curLevelIsFinished = false;

    private static string[] levels   = {"level1", "level2", "level3", "level4", "level5" };
    private string          mainMenu = "MainMenu";
    private SaveData        data;
    private GameObject      Player;
    private float           timer        = 0f;
    private int             currentLevel = -1;
    private GameObject      lights;
    private GameObject      finishPortal;
    private TextMeshProUGUI TimeText;
    private LightContainerController lcon;
    

    void Awake()
    {
        if      (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }

        DontDestroyOnLoad(transform.gameObject);

        //######################################### uncomment before release#################################################
        //data = SaveSystem.LoadPlayer();    
        data = SaveSystem.CreateNewSaveData();

        TimeText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        TimeText.gameObject.SetActive(false);

        if (data == null)
        {
            Debug.Log("Creating new save data.");
            data = SaveSystem.CreateNewSaveData();
        }        
    }

    private void Update()
    {
        if (Player       == null) Player       = GameObject.Find("Player");
        if (finishPortal == null) finishPortal = GameObject.Find("Finish Portal");
        if (lights       == null) lights       = GameObject.Find("Lights");
        if (lcon == null)
        {
            if (Player != null) lcon = Player.transform.Find("LightContainer").GetComponent<LightContainerController>();
        }
        if (!curLevelIsFinished) timer += Time.deltaTime;

        if(Player != null)
        {
            int numLightsCollected = lcon.GetLightCount();
            if (AllLightsCollected(numLightsCollected))
                finishPortal.SetActive(true);
            else
                finishPortal.SetActive(false);
        }



        TimeText.text = (Mathf.Round(timer * 100f) / 100f).ToString() + " s";

        //Debug.Log("Light Movement Factor ------------> " + lcon.GetLightMovementFactor());
    }

    public void PlayerWon()
    {
        curLevelIsFinished = true;
        Player.SetActive(false);

        int   lightsCollected = Player.transform.Find("LightContainer").GetComponent<LightContainerController>().GetLightCount();
        float bestTime        = timer;

        bool collectedMoreLights = data.lightsCollectedPerLevel[currentLevel] < lightsCollected;
        bool fasterThanPrevious  = data.levelTimes[currentLevel] > timer || data.levelTimes[currentLevel] < 0;

        if (!collectedMoreLights) lightsCollected = data.lightsCollectedPerLevel[currentLevel];
        if (!fasterThanPrevious)  bestTime        = data.levelTimes[currentLevel];

        data.UpdateDataAt(bestTime, lightsCollected, currentLevel);
        Save();
    }

    public void GoToNextScene()
    {
        curLevelIsFinished = false;
        currentLevel = (currentLevel + 1) % levels.Length;
        SceneManager.LoadScene(levels[currentLevel]);

        Player       = GameObject.Find("Player");
        finishPortal = GameObject.Find("Finish Portal");
        lights       = GameObject.Find("Lights");

        timer = 0;
        TimeText.gameObject.SetActive(true);
    }

    public void GoToMainMenu()
    {
        curLevelIsFinished = true;
        SceneManager.LoadScene(mainMenu);
        TimeText.gameObject.SetActive(false);
    }

    public void Save()
    {
        Debug.Log("######################### Saving Game... #########################");
        Debug.Log("Current Level: " + currentLevel);
        for (int i = 0; i < levels.Length; i++)
        {
            Debug.Log("             Data for Level: " + i);
            Debug.Log("                 Level Time: " + data.levelTimes[i]);
            Debug.Log("           Lights Collected: " + data.lightsCollectedPerLevel[i]);
            Debug.Log("                 Completed?: " + data.completedLevels[i]);
        }
        Debug.Log("######################### ... Game Saved #########################");
        
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
        currentLevel = level - 1;
        SceneManager.LoadScene(levels[level - 1]);

        Player       = GameObject.Find("Player");
        finishPortal = GameObject.Find("Finish Portal");
        lights       = GameObject.Find("Lights");

        timer = 0;
        curLevelIsFinished = false;
        TimeText.gameObject.SetActive(true);
    }

    private bool AllLightsCollected(int collected)
    {
        int numLightsInCurLevel = lights.transform.childCount;
        if (collected == numLightsInCurLevel) return true;
        return false;
    }
}
