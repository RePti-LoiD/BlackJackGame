using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class UserDataWriter : MonoBehaviour
{
    [SerializeField] private GamePrefs prefs;

    public static void UpdateUserData(User user)
    {
        if (!File.Exists(GamePrefs.GetSavePath()))
            File.CreateText(GamePrefs.GetSavePath());

        File.WriteAllText(GamePrefs.GetSavePath(), JsonConvert.SerializeObject(user));
    }
}