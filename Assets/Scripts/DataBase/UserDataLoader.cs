using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class UserDataLoader : MonoBehaviour
{
    [Obsolete]
    public static UserDataLoader Instance { get; private set; } = null;

    private static User userData;
    public static User UserData { get => userData; private set => userData = value; }

    private FirebaseAuth firebaseAuth;
    private DatabaseReference databaseReference;

    public Action<User> OnDataLoad;

    private void Start()
    {
        Instance = this;

        firebaseAuth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (firebaseAuth.CurrentUser != null)
            StartCoroutine(CollectUserData());
    }

    public void LoadUserBalance(float count)
    {
        databaseReference.Child("users").Child(firebaseAuth.CurrentUser.UserId).Child("PlayerBalance").SetValueAsync(count);
    }

    public void UpdateUserData()
    {
        StartCoroutine(CollectUserData());
    }

    private IEnumerator CollectUserData()
    {
        Task<DataSnapshot> result = databaseReference
            .Child("users")
            .Child(firebaseAuth.CurrentUser.UserId)
            .GetValueAsync();

        yield return new WaitUntil(() => result.IsCompleted);

        userData = JsonConvert.DeserializeObject<User>(result.Result.GetRawJsonValue());

        OnDataLoad?.Invoke(userData);

        StopAllCoroutines();
    }
}