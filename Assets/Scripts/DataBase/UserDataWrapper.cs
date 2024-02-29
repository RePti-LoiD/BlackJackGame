using Newtonsoft.Json;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UserDataWrapper
{
    private static User data;
    private static bool isChanged;

    public static User UserData
    {
        get
        {
            if (!File.Exists(GamePrefs.GetSavePath())) return null;

            if (data == null || isChanged)
                LoadData();

            return data;
        }
        set
        {
            isChanged = true;
            data = value;
            SaveData();
        }
    }

    private static void LoadData()
    {
        data = JsonConvert.DeserializeObject<User>(File.ReadAllText(GamePrefs.GetSavePath()));
       
        GUIUtility.systemCopyBuffer = GamePrefs.GetSavePath();

        data.UserWallet.OnWalletMoneyChanged += (balance) => SaveData();
    }

    private static void SaveData() =>
        File.WriteAllText(GamePrefs.GetSavePath(), JsonConvert.SerializeObject(data));
}