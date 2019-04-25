using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

    public int currentLevel;
    public int maxLevelReached;
    public float[] levelTimes;
    public int[] lightsCollectedPerLevel;

    public SaveData(int curLevel, int maxLevel, float[] times, int[] lightsCollected)
    {
        currentLevel            = curLevel;
        maxLevelReached         = maxLevel;
        levelTimes              = times;
        lightsCollectedPerLevel = lightsCollected;
    }

    public void UpdateDataAt(float time, int lightsCollected, int index)
    {
        levelTimes[index] = time;
        lightsCollectedPerLevel[index] = lightsCollected;
    }
}
