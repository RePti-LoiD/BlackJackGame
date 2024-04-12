public class BJClientBet : BJBet
{
    public void ChangeBet(float multiplier)
    {
        int newBet = (int)(CurrentBet * multiplier);

        if (newBet > UserDataWrapper.UserData.UserWallet.Balance) return;

        OnBetSet?.Invoke(newBet, CurrentBet);
        CurrentBet = (int)(CurrentBet * multiplier);
    }
}