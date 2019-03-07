using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

    private static string path = Application.persistentDataPath + "/savedata.poop";

    public static void SavePlayerData(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

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
        float[] times = new float[GameController.instance.GetNumLevels()];
        for (int i = 0; i < times.Length; i++)
        {
            times[i] = -1;
        }
        return new SaveData(0,0, times);
    }
	
}
