using Firebase.Auth;
using Firebase.Database;
using System;
using TMPro;
using UnityEngine;

public class UserScore : MonoBehaviour
{
    [SerializeField] private UserDataLoader userDataLoader;
    [SerializeField] private MainMenuCardsHandler mainMenuCardsHandler;
    [SerializeField] private TextMeshProUGUI scoreText;

    private DatabaseReference databaseReference;
    private FirebaseAuth firebaseAuth;

    private void Start()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.UserScore) && PlayerPrefs.GetString(PlayerPrefsKeys.IsGuest) == IsGuest.Guest.ToString())
            scoreText.text = PlayerPrefs.GetInt(PlayerPrefsKeys.UserScore).ToString();

        mainMenuCardsHandler.OnCardSwipe += (cardWeight) =>
        {
            scoreText.text = (Convert.ToInt32(scoreText.text) + cardWeight).ToString();

            if (PlayerPrefs.GetString(PlayerPrefsKeys.IsGuest) == IsGuest.Guest.ToString())
                PlayerPrefs.SetInt(PlayerPrefsKeys.UserScore, Convert.ToInt32(scoreText.text));
            else
                databaseReference.Child("users").Child(firebaseAuth.CurrentUser.UserId).Child("PlayerBalance").SetValueAsync(scoreText.text);
        };

        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        firebaseAuth = FirebaseAuth.DefaultInstance;

        userDataLoader.OnDataLoad += (userData) =>
        {
            scoreText.text = userData.PlayerBalance.ToString();
        };
    }
}