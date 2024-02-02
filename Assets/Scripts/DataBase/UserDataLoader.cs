using System;
using UnityEngine;

public class UserDataLoader : MonoBehaviour
{
    [Obsolete]
    public static UserDataLoader Instance { get; private set; } = null;

    private static User userData;
    public static User UserData { get => userData; private set => userData = value; }

    public Action<User> OnDataLoad;

    private void Start()
    {
        Instance = this;
    }
}