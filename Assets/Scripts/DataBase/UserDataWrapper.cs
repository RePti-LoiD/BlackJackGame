using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class UserDataWrapper
{
    private static User data;
    private static bool isChanged;

    public static User UserData
    {
        get
        {
            if (!File.Exists(GamePrefs.SavePath)) return null;

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
        data = JsonConvert.DeserializeObject<User>(File.ReadAllText(GamePrefs.SavePath));
        Debug.Log(File.ReadAllText(GamePrefs.SavePath));

        data.UserWallet.OnWalletMoneyChanged += (balance) => SaveData();
    }

    private static void SaveData() =>
        File.WriteAllText(GamePrefs.SavePath, JsonConvert.SerializeObject(data));
}