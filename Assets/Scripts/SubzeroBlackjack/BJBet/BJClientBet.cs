public class BJClientBet : BJBet
{
    public void ChangeBet(float multiplier)
    {
        int newBet = (int)(CurrentBet * multiplier);

        if (newBet > (UserDataWrapper.UserData.UserWallet.Balance / 2)) return;

        OnBetSet?.Invoke(newBet, CurrentBet);
        CurrentBet = (int)(CurrentBet * multiplier);
    }
}