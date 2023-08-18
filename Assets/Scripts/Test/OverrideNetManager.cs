using System;
using Mirror;
using TMPro;
using UnityEngine;

public class OverrideNetManager : NetworkManager
{    
    [SerializeField] private TMP_InputField userNameText;

    public Action OnLastClientConnected;
    private int clientCount;

    public override void Start()
    {
        base.Start();

        userNameText.onEndEdit.AddListener((text) =>
        {
            if (!string.IsNullOrEmpty(text))
                UserData.Init(Guid.NewGuid(), text);
        });
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        clientCount++;
        
        if (clientCount == maxConnections)
            OnLastClientConnected?.Invoke();

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