using TMPro;
using UnityEngine;

public class UserDataVisualization : MonoBehaviour
{
    [SerializeField] private TMP_Text nickNameText;
    [SerializeField] private TMP_Text firstNameText;
    [SerializeField] private TMP_Text balanceText;

    [Header("other")]
    [SerializeField] private string currencyChar;

    public void VisualizeUserData(User data)
    {
        if (balanceText != null)
        { 
            data.UserWallet.OnWalletMoneyChanged += (balance) =>
            {
                balanceText.text = data.UserWallet.Balance.ToString() + currencyChar;
            };
        }

        if (nickNameText.text != null) nickNameText.text = data.NickName;
        if (firstNameText.text != null) firstNameText.text = data.FirstName;

        if (balanceText.text != null) balanceText.text = data.UserWallet.Balance.ToString() + currencyChar;
    }
}