using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

    private static string path = Application.persistentDataPath + "/savedata.poop";

    public static void SavePlayerData(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream      stream    = new FileStream(path, FileMode.Create);

        //SaveData data = new SaveData(curLevel, maxLevel, times);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadPlayer()
    {
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static SaveData CreateNewSaveData()
    {
        File.Delete(path);
        int numLevels = GameController.instance.GetNumLevels();

        float[]    times  = new float[numLevels];
        int  []    lights = new int  [numLevels];
        bool [] completed = new bool [numLevels];

        for (int i = 0; i < numLevels; i++)
        {
            times[i]     = -1;
            lights[i]    = 0;
            completed[i] = false;
        }
        return new SaveData(times, lights, completed);
    }
	
}
