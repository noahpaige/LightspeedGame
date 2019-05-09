using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelGridItemInfo : MonoBehaviour
{
    public int LevelNumber;
    public int NumberLightsInLevel;

    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI LightsText;


    private void Start()
    {
        SaveData data = GameController.instance.GetSaveData();


        Debug.Log("Size of lighst list: " + data.lightsCollectedPerLevel.Length.ToString());
        Debug.Log("Size of times list: " + data.levelTimes.Length);
        Debug.Log("Gamecontroller num levels: " + GameController.instance.GetNumLevels());

        LevelText.SetText("Level " + LevelNumber.ToString());

        float levelTime = data.levelTimes[LevelNumber - 1];
        if(levelTime < 0) TimeText.SetText("Time: --");
        else              TimeText.SetText("Time: " + string.Format("{0:.##}", levelTime)); //round to two decimal places

        int lightsCollected = data.lightsCollectedPerLevel[LevelNumber - 1];
        LightsText.SetText("Lights: " + lightsCollected.ToString() + "/" + NumberLightsInLevel);
    }

    public void GoToLevel()
    {
        GameController.instance.GoToGivenLevel(LevelNumber);
    }

}
