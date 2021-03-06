﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    public GameObject levelMenuItemPrefab;
    public GameObject ScrollviewContent;
    private void Awake()
    {
        string[] levels = GameController.instance.GetLevelList();
        for(int i = 1; i < levels.Length + 1; i++)
        {
            GameObject menuItem = Instantiate(levelMenuItemPrefab);
            menuItem.GetComponent<LevelGridItemInfo>().LevelNumber = i;
            menuItem.transform.SetParent(ScrollviewContent.transform);
        }
    }
    public void Refresh()
    {
        SaveData data = GameController.instance.GetSaveData();
        while(data == null) data = GameController.instance.GetSaveData();

        foreach (Transform button in transform)
        {
            int levelNum = button.GetComponent<LevelGridItemInfo>().LevelNumber - 1;

            //Debug.Log("Completed level " + (levelNum + 1) + "? --> " + data.completedLevels[levelNum]);

            if (levelNum == 0)
            {
                button.GetComponent<Button>().interactable = true;
            }
            else if (data.completedLevels[levelNum])
            {
                button.GetComponent<Button>().interactable = true;
            }
            else if (data.completedLevels[levelNum - 1])
            {
                button.GetComponent<Button>().interactable = true;
            }
            else
            {
                button.GetComponent<Button>().interactable = false;
            }
            button.GetComponent<LevelGridItemInfo>().UpdateLevelInfo();
        }
    }
}
