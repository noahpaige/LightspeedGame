using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

    public float[] levelTimes;
    public int  [] lightsCollectedPerLevel;
    public bool [] completedLevels;

    public SaveData(float[] times, int[] lightsCollected, bool[] completed)
    {
        levelTimes              = times;
        lightsCollectedPerLevel = lightsCollected;
        completedLevels         = completed;
    }

    public void UpdateDataAt(float time, int lightsCollected, int index)
    {
        levelTimes[index]              = time;
        lightsCollectedPerLevel[index] = lightsCollected;
        completedLevels[index]         = true;
    }
}
