using TMPro;
using UnityEngine;

public class UserDataVisualization : MonoBehaviour
{
    [SerializeField] private TMP_Text nickNameText;
    [SerializeField] private TMP_Text firstNameText;
    [SerializeField] private TMP_Text balanceText;

    [Header("other")]
    [SerializeField] private string currencyChar;

    public User currentUser { get; private set;  }

    public void VisualizeUserData(User data)
    {
        currentUser = data;

        if (balanceText != null && data.UserWallet != null)
            data.UserWallet.OnWalletMoneyChanged += (balance) =>
            {
                balanceText.text = data.UserWallet.Balance.ToString() + currencyChar;
            };

        if (nickNameText.text != null) nickNameText.text = data.NickName;
        if (firstNameText.text != null) firstNameText.text = data.FirstName;

        if (balanceText.text != null && data.UserWallet != null) balanceText.text = data.UserWallet.Balance.ToString() + currencyChar;
    }

    public void UpdateData()
    {
        VisualizeUserData(currentUser);
    }
}