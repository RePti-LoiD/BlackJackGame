using System.IO;
using UnityEngine;

public class GamePrefs
{
    public static string SavePath {
        get
        {
            return Path.Combine(Application.persistentDataPath, "UserProfileData.json");
        }
    }

    public static string EditorSavePath {
        get
        {
            return Path.Combine(Application.persistentDataPath, "EditorUserProfileData.json");
        }
    }

    public static string GetSavePath()
    {
        string savePath = SavePath;

#if UNITY_EDITOR
                savePath = EditorSavePath;
#endif

        return savePath;
    }

    public static int DefaultBalance { get => 5000; }
}