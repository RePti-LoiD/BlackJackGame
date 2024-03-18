using UnityEngine;
using TMPro;

public class BJPlayerDataVizualization : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nickNameText;
    [SerializeField] private TextMeshProUGUI firstNameText;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI betText;

    [Header("other")]
    [SerializeField] private string currencyChar;

    private BJPlayer currentPlayer;

    public void VisualizeBJPlayerData(BJPlayer data)
    {
        currentPlayer = data;

        if (balanceText != null && data.UserData.UserWallet != null)
            data.UserData.UserWallet.OnWalletMoneyChanged += (balance) =>
            {
                balanceText.text = data.UserData.UserWallet.Balance.ToString() + currencyChar;
            };

        if (nickNameText.text != null) nickNameText.text = data.UserData.NickName;
        if (firstNameText.text != null) firstNameText.text = data.UserData.FirstName;

        if (balanceText.text != null && data.UserData.UserWallet != null) balanceText.text = data.UserData.UserWallet.Balance.ToString() + currencyChar;
   
        betText.text = data.PlayerBet.ToString();
    }

    public void UpdateData()
    {
        VisualizeBJPlayerData(currentPlayer);
    }
}