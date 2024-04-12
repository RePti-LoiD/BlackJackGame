using TMPro;
using UnityEngine;

public class BJServerBet : BJBet
{
    [SerializeField] private TMP_InputField betField;

    protected override void Start()
    {
        base.Start();

        betField.onEndEdit.AddListener((data) =>
        {
            print(data);
            CurrentBet = int.Parse(data);

            OnBetSet?.Invoke(CurrentBet, CurrentBet);
        });
        
        betField.onValueChanged.AddListener((data) =>
        {
            int parsedBet = int.Parse(data);

            print(parsedBet);

            if (parsedBet * 2 >= UserDataWrapper.UserData.UserWallet.Balance)
                betField.text = (UserDataWrapper.UserData.UserWallet.Balance / 2).ToString();
        });
    }

    public void SetBet(TextMeshProUGUI currentBetText)
    {
        print(currentBetText.text.Length);

        CurrentBet = int.Parse(currentBetText.text.Trim());

        OnBetSet?.Invoke(CurrentBet, CurrentBet);
    }
}
