using System;
using UnityEngine;

public class TestServerInvoker : MonoBehaviour
{
    [SerializeField] private BJPlayer local;
    [SerializeField] private BJPlayer external;

    private void Awake()
    {
        local.UserData = UserDataWrapper.UserData;
        external.UserData = new User("Bot", "Botyara", "", DateTimeOffset.UtcNow.ToUnixTimeSeconds(), new Wallet(1000));
    }
}