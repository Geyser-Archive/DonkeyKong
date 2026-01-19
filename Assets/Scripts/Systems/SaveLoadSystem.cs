using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadSystem
{
    private static readonly string path = Application.persistentDataPath + "/Data/";
    private static readonly string extension = ".data"; // Every file extension is allowed

    public static void SaveGame()
    {
        SaveLoadSystem.Save(Game.GameData, "Game");
    }

    public static void SaveSettings()
    {
        SaveLoadSystem.Save(Settings.SettingsData, "Settings");
    }

    public static GameData LoadGame()
    {
        return SaveLoadSystem.Load("Game") as GameData;
    }

    public static SettingsData LoadSettings()
    {
        return SaveLoadSystem.Load("Settings") as SettingsData;
    }

    private static void Save(object data, string name)
    {
        string item = SaveLoadSystem.Item(name);

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(item, FileMode.Create);

        try
        {
            binaryFormatter.Serialize(fileStream, data);

            fileStream.Close();
        }
        catch
        {
            fileStream.Close();

            if (File.Exists(item))
            {
                File.Delete(item);
            }
        }
    }

    private static object Load(string name)
    {
        string item = SaveLoadSystem.Item(name);

        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(item, FileMode.Open);

            object data = binaryFormatter.Deserialize(fileStream);

            fileStream.Close();

            return data;
        }
        catch
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return null;
        }
    }

    private static string Item(string name)
    {
        return SaveLoadSystem.path + name + SaveLoadSystem.extension;
    }
}