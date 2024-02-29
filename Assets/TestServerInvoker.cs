using System;
using TMPro;
using UnityEngine;

public class TestServerInvoker : MonoBehaviour
{
    [SerializeField] private BJPlayer local;
    [SerializeField] private BJPlayer external;
    [SerializeField] private TextMeshProUGUI localPlayerIDText;
    [SerializeField] private TextMeshProUGUI enemyPlayerIDText;

    private void Awake()
    {
        local.UserData = UserDataWrapper.UserData;
        external.UserData = new User("Bot", "Botyara", "", DateTimeOffset.UtcNow.ToUnixTimeSeconds(), new Wallet(1000));
        
        ShowGuid();
    }

    public void ShowGuid()
    {
        localPlayerIDText.text = local.UserData.Id.ToString();
        enemyPlayerIDText.text = external.UserData.Id.ToString();
    }
}