public class BJClientBet : BJBet
{
    public void ChangeBet(float multiplier)
    {
        CurrentBet = (int)(CurrentBet * multiplier);
        print(CurrentBet);

        OnBetSet?.Invoke(CurrentBet);
    }
}