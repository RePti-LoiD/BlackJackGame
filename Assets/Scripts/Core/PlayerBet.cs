using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentMoney;
    [SerializeField] private TMP_InputField betCount;
    [SerializeField] private TextMeshProUGUI remainsMoney;
    [SerializeField] private Button betButton;

    public Action<int, User> OnPlayerBet;

    private void Start()
    {
        betCount.onValueChanged.AddListener((data) =>
        {
            if (string.IsNullOrEmpty(data)) return;

            remainsMoney.text = $"{(int)UserDataWrapper.UserData.UserWallet.Balance - Convert.ToInt32(data)}$";
        });

        betButton.onClick.AddListener(() =>
        {
            
            if ((int)UserDataWrapper.UserData.UserWallet.Balance - Convert.ToInt32(betCount.text) < 0) return;

            OnPlayerBet?.Invoke(Convert.ToInt32(betCount.text), UserDataWrapper.UserData);

            gameObject.transform.parent.gameObject.SetActive(false);
        });

        currentMoney.text = $"{UserDataWrapper.UserData.UserWallet.Balance}$";
    }
}