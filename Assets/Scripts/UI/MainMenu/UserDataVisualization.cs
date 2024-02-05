using TMPro;
using UnityEngine;

public class UserDataVisualization : MonoBehaviour
{
    [SerializeField] private User userData;

    [SerializeField] private TMP_Text nickNameText;
    [SerializeField] private TMP_Text firstNameText;
    [SerializeField] private TMP_Text balanceText;

    [Header("other")]
    [SerializeField] private string currencyChar;

    private void Start()
    {
        UserDataWrapper.UserData.UserWallet.OnWalletMoneyChanged += (balance) =>
        {
            balanceText.text = UserDataWrapper.UserData.UserWallet.Balance.ToString() + currencyChar;
        };

        nickNameText.text = UserDataWrapper.UserData.NickName;
        firstNameText.text = UserDataWrapper.UserData.FirstName;

        balanceText.text = UserDataWrapper.UserData.UserWallet.Balance.ToString() + currencyChar;
    }
}