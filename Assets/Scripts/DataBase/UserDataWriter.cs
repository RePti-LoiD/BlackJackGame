using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class UserDataWriter : MonoBehaviour
{
    [SerializeField] private GamePrefs prefs;

    public static void UpdateUserData(User user)
    {
        if (!File.Exists(GamePrefs.SavePath))
            File.CreateText(GamePrefs.SavePath);

        File.WriteAllText(GamePrefs.SavePath, JsonConvert.SerializeObject(user));
    }
}