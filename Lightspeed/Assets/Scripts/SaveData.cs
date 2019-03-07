using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

    public int currentLevel;
    public int maxLevelReached;
    public float[] levelTimes;

    public SaveData(int curLevel, int maxLevel, float[] times)
    {
        currentLevel = curLevel;
        maxLevelReached = maxLevel;
        levelTimes = times;
    }

    public void UpdateTimeAt(float time, int index)
    {
        levelTimes[index] = time;
    }
}
