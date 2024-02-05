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

    public static int DefaultBalance { get => 5000; }
}