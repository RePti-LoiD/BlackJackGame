using System;
using Mirror;
using TMPro;
using UnityEngine;

public class OverrideNetManager : NetworkManager
{    
    [SerializeField] private TMP_InputField userNameText;

    private UserData userData;

    public override void Start()
    {
        base.Start();

        userNameText.onEndEdit.AddListener((text) =>
        {
            if (!string.IsNullOrEmpty(text))
            {
                userData = UserData.Init(Guid.NewGuid(), text);

                Debug.Log("User data added");
            }
        });
    }
}

public class UserData : NetworkMessage
{
    public static readonly UserData Data = new UserData();

    public Guid Id;
    public string Name;

    public static UserData Init(Guid id, string name)
    {
        Data.Id = id;
        Data.Name = name;

        return Data;
    } 
}