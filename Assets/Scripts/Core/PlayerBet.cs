using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBet : MonoBehaviour
{
    [SerializeField] private UserDataLoader userDataLoader; 
    [SerializeField] private TextMeshProUGUI currentMoney;
    [SerializeField] private TMP_InputField betCount;
    [SerializeField] private TextMeshProUGUI remainsMoney;
    [SerializeField] private Button betButton;

    public Action<int, User> OnPlayerBet;

    private User userData;

    private void Start()
    {
        betCount.onValueChanged.AddListener((data) =>
        {
            if (string.IsNullOrEmpty(data)) return;

            if (PlayerPrefs.GetString(PlayerPrefsKeys.IsGuest) == IsGuest.Guest.ToString())
            {
                remainsMoney.text = $"{PlayerPrefs.GetInt(PlayerPrefsKeys.UserScore) - Convert.ToInt32(data)}$";
            }
            else
            {
                remainsMoney.text = $"{this.userData.PlayerBalance - Convert.ToInt32(data)}$";
            }
        });

        betButton.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetString(PlayerPrefsKeys.IsGuest) == IsGuest.Guest.ToString())
            {
                if (PlayerPrefs.GetInt(PlayerPrefsKeys.UserScore) - Convert.ToInt32(betCount.text) < 0) return;

                OnPlayerBet?.Invoke(Convert.ToInt32(betCount.text), new User()
                {
                    FirstName = "Player",
                    NickName = "Guest",
                    PlayerBalance = PlayerPrefs.GetInt(PlayerPrefsKeys.UserScore)
                });
            }
            else
            {
                if (userData.PlayerBalance - Convert.ToInt32(betCount.text) < 0) return;

                OnPlayerBet?.Invoke(Convert.ToInt32(betCount.text), userData);;
            }
            gameObject.transform.parent.gameObject.SetActive(false);
        });

        if (PlayerPrefs.GetString(PlayerPrefsKeys.IsGuest) == IsGuest.Guest.ToString())
            currentMoney.text = $"{PlayerPrefs.GetInt(PlayerPrefsKeys.UserScore)}$";

        userDataLoader.UpdateUserData();

        userDataLoader.OnDataLoad += (userData) =>
        {
            this.userData = userData;
            currentMoney.text = $"{this.userData.PlayerBalance}$";
        };
    }
}