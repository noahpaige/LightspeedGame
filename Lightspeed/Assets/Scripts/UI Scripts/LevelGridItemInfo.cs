﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LevelGridItemInfo : MonoBehaviour
{
    public int LevelNumber;
    //public int NumberLightsInLevel;

    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI TimeText;

    public MenuSoundController msc;


    private Button btn;


    private void Start()
    {
        UpdateLevelInfo();
        btn = GetComponent<Button>();
    }

    public void GoToLevel()
    {
        GameController.instance.GoToGivenLevel(LevelNumber);
    }

    public void UpdateLevelInfo()
    {
        SaveData data = GameController.instance.GetSaveData();


        //Debug.Log("Size of lighst list: " + data.lightsCollectedPerLevel.Length.ToString());
        //Debug.Log("Size of times list: " + data.levelTimes.Length);
        //Debug.Log("Gamecontroller num levels: " + GameController.instance.GetNumLevels());

        LevelText.SetText("Level " + LevelNumber.ToString());

        float levelTime = Mathf.Round(data.levelTimes[LevelNumber - 1] * 100f) / 100f;
        if (levelTime < 0) TimeText.SetText("Time: --");
        else TimeText.SetText("Time: " + levelTime); // string.Format("{0:.##}", levelTime)); //round to two decimal places

        //int lightsCollected = data.lightsCollectedPerLevel[LevelNumber - 1];
        //LightsText.SetText("Lights: " + lightsCollected.ToString() + "/" + NumberLightsInLevel);
    }

    public void CheckedPlayHoverSound()
    {
        if (btn.interactable) msc.PlayHoverSound();
    }

    public void CheckedPlaySelectSound()
    {
        if (btn.interactable) msc.PlaySelectSound();
    }
}
